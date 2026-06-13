using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Functional.Extensions;
using ScrubJay.Text.Collections;
using ScrubJay.Text.Debugging;

namespace ScrubJay.Text.Rendering;

internal sealed record class RenderToMethodInfo(MethodInfo Method, Type InstanceType, int Priority);

internal static class RenderToCache
{
    private static readonly RenderToMethodInfo[] _renderToMethods;
    private static readonly ConcurrentTypeMap<Action<object, TextBuilder>> _boxedRenderTos = new();

    static RenderToCache()
    {
        // As soon as we startup we register all [RenderToMethod] methods
        _renderToMethods = AppDomain
            .CurrentDomain
            // all assemblies I can see    
            .GetAssemblies()
            .Where(FilterAssembly)
            // get all exported types (public)
            .SelectMany(EnumerateAssemblyExportedTypes)
            // get all methods with [RenderToMethod] and a compatible signature
            .SelectMany(EnumerateRenderToMethods)
            // we allow for overloaded priority for users
            .OrderByDescending(static method => method.Priority)
            .ToArray();
    }

    #region RenderToMethod Search
    private static bool FilterAssembly(Assembly assembly)
    {
        // assemblies can throw at runtime
        try
        {
            // cannot load types from dynamic assemblies
            if (assembly.IsDynamic)
                return false;
            // skip anything from Microsoft or System
            string fullname = assembly.FullName!;
            if (fullname.StartsWith("Microsoft", StringComparison.Ordinal) ||
                fullname.StartsWith("System", StringComparison.Ordinal))
                return false;
            return true;
        }
        catch (Exception ex)
        {
            // skip this assembly
            Trouble.Error(ex);
            Trouble.Break();
            return false;
        }
    }

    private static IEnumerable<Type> EnumerateAssemblyExportedTypes(Assembly assembly)
    {
        try
        {
            // public types visible outside the assembly
            return assembly.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException typeLoadException)
        {
            // the types we could load
            return typeLoadException.Types.WhereNotNull();
        }
        catch (Exception ex)
        {
            Trouble.Error(ex);
            Trouble.Break();
            return Type.EmptyTypes;
        }
    }

    private static IEnumerable<RenderToMethodInfo> EnumerateRenderToMethods(Type type) => type
        // public static methods declared exactly on this type
        .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .SelectWhere<MethodInfo, RenderToMethodInfo>(static method =>
        {
            // has the attribute
            var renderToMethodAttribute = method.GetCustomAttribute<RenderToMethodAttribute>();
            if (renderToMethodAttribute is null)
                return None;
            int priority = renderToMethodAttribute.Priority;

            // void return type
            if (method.ReturnType != typeof(void))
                return None;

            // two parameters exactly
            var methodParameters = method.GetParameters();
            if (methodParameters.Length != 2)
                return None;

            // the first must not be out
            var valueParameter = methodParameters[0];
            if (valueParameter.IsOut)
                return None;
            var instanceType = valueParameter.ParameterType;

            // the second must be TextBuilder (no in nor out)
            var tbParameter = methodParameters[1];
            if (tbParameter.ParameterType != typeof(TextBuilder) ||
                tbParameter.IsIn || tbParameter.IsOut)
                return None;

            // we capture the instance type and the priority as well
            return new RenderToMethodInfo(method, instanceType, priority);
        });
    #endregion


    private static Option<int> InputConversionSpecificity(Type inputType, Type destinationType)
    {
        // exact > subclass > interface > object
        if (inputType == destinationType)
            return 100;

        if (destinationType.IsGenericParameter)
        {
            // pass the constraints
            var constraints = destinationType.GetGenericParameterConstraints();
            if (constraints.All(c => inputType.IsAssignableTo(c)))
            {
                return 99;
            }
            return None;
        }

        if (destinationType.IsValueType)
            return None;

        // check if assignable
        // see Type.Helpers.IsAssignableFrom

        var destSystemType = destinationType.UnderlyingSystemType;
        if (destSystemType.Name == "RuntimeType")
        {
            return InputConversionSpecificity(inputType, destSystemType);
        }

        // destination type must somehow be 'under' input type
        int classDepth = 0;

        Type? inType = inputType;
        while (inType is not null)
        {
            if (destinationType.IsClass)
            {
                if (destinationType == inType)
                {
                    if (destinationType != typeof(object))
                        return 90 - classDepth;
                    return 1; // worst match
                }
            }
            else if (destinationType.IsInterface)
            {
                var inTypeInterfaces = inType.GetInterfaces();
                for (int i = 0; i < inTypeInterfaces.Length; i++)
                {
                    if (inTypeInterfaces[i] == destinationType)
                        return 90 - classDepth - (5 * i);
                }
            }
            else
            {
                Debugger.Break();
                return None;
            }

            inType = inType.BaseType;
            classDepth++;
        }

        return None;
    }

    private static Option<(MethodInfo ConcreteMethod, int Specificity)> MatchSpecificity(MethodInfo renderToMethod, Type valueType)
    {
        // default is not matching at worst specificity
        MethodInfo? concreteMethod = null;
        int specificity = 0;


        // we need to examine the value parameter of the method
        var renderToMethodParameters = renderToMethod.GetParameters();
        Debug.Assert(renderToMethodParameters.Length == 2);
        var methodValueParameter = renderToMethodParameters[0];
        var methodValueType = methodValueParameter.ParameterType;

        if (renderToMethod.IsGenericMethodDefinition)
        {
            var genericTypes = renderToMethod.GetGenericArguments();
            Debug.Assert(genericTypes.Length >= 1);

            var t = genericTypes[0];
            var tAttributes = t.GenericParameterAttributes;

#if NET9_0_OR_GREATER
            if (valueType.IsByRefLike)
            {
                if (!tAttributes.HasFlag(GenericParameterAttributes.AllowByRefLike))
                {
                    return None;
                }
            }

            tAttributes &= ~GenericParameterAttributes.AllowByRefLike;
#endif

            if (tAttributes > GenericParameterAttributes.None)
                Debugger.Break();

            var tConstraints = t.GetGenericParameterConstraints();
            if (tConstraints.Length > 0)
            {
                var all = tConstraints.All(tc => valueType.IsAssignableTo(tc));

                if (!all)
                    return None;
            }

            // check for array variance
            if (methodValueType.IsArray || valueType.IsArray)
            {
                if (methodValueType.IsArray != valueType.IsArray)
                    return None;

                var methodArrayValueElementType = methodValueType.GetElementType()!;
                var methodArrayValueRank = methodValueType.GetArrayRank();
                var arrayValueElementType = valueType.GetElementType()!;
                var arrayValueRank = valueType.GetArrayRank();

                if (arrayValueRank != methodArrayValueRank)
                    return None;

                if (methodArrayValueElementType.IsGenericParameter)
                {
                    // this is great!
                    concreteMethod = Result.Try(() => renderToMethod.MakeGenericMethod(arrayValueElementType)).OkOrDefault();
                    specificity = 97;
                    goto end;
                }

                // have to be assignable
                if (InputConversionSpecificity(arrayValueElementType, methodArrayValueElementType)
                    .IsSome(out specificity))
                {
                    concreteMethod = Result.Try(() => renderToMethod.MakeGenericMethod(arrayValueElementType)).OkOrDefault();
                    goto end;
                }

                // fail
                return None;
            }

            concreteMethod = Result.Try(() => renderToMethod.MakeGenericMethod(valueType)).OkOrDefault();

        }

        if (InputConversionSpecificity(valueType, methodValueType).IsSome(out var spec))
        {
            specificity = spec;
            concreteMethod ??= renderToMethod;
            goto end;
        }

end:
        if (concreteMethod is null)
            return None;
        return Some((ConcreteMethod: concreteMethod, Specificity: specificity));
    }

    internal static Action<object, TextBuilder> GetObjectRenderTo(Type type)
    {
        return _boxedRenderTos.GetOrAdd(type, static t =>
        {
            if (typeof(IRenderable).IsAssignableFrom(t))
                return static (obj, builder) => ((IRenderable)obj).RenderTo(builder);

            var resolveBoxed = typeof(Renderer)
                .GetMethod(nameof(BoxedCacheAction), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(t);

            return (Action<object, TextBuilder>)resolveBoxed.Invoke(null, null)!;
        });
    }

    private static Action<object, TextBuilder> BoxedCacheAction<T>()
    {
        var concreteAction = RenderToCache<T>.Invoke;
        return (obj, builder) => concreteAction((T)obj, builder);
    }


    private static Action<T, TextBuilder>? GetRenderToFor<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var instanceType = typeof(T);

        // Look for a suitable method
        MethodInfo? renderToMethod = null;

        if (instanceType == typeof(object))
        {
            renderToMethod = typeof(RenderToCache)
                .GetMethod(nameof(GetObjectRenderTo), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }
        else
        {
            // check if the instance type has a RenderTo method
            renderToMethod = instanceType
                .FindMatchingInstanceMethods("RenderTo", typeof(void), [typeof(TextBuilder)])
                .FirstOrDefault();

            if (renderToMethod is null)
            {
                // check for a compatible registered method
                var matchingMethods = _renderToMethods
                    .SelectWhere(method => MatchSpecificity(method.Method, instanceType))
                    .OrderByDescending(pair => pair.Specificity)
                    //.Select(pair => pair.ConcreteMethod)
                    .ToList();

                if (matchingMethods.Count == 0)
                {
                    Trouble.Hold();
                    return null;
                }

                if (matchingMethods.Count > 1)
                {
                    Trouble.Hold();
                }

                renderToMethod = matchingMethods.First().ConcreteMethod;
            }
        }

        if (renderToMethod is null)
            return null;

        try
        {
            var delType = typeof(Action<T, TextBuilder>);
            var del = Delegate.CreateDelegate(delType, renderToMethod);
            return (Action<T, TextBuilder>)del;
        }
        catch (Exception ex)
        {
            Trouble.Warn(ex);
            Debugger.Break();
            // ignore all issues
            return null;
        }

    }
}

internal static class RenderToCache<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    internal delegate void AnyRenderTo(in T instance, TextBuilder builder);

    internal static readonly AnyRenderTo Invoke;

    static RenderToCache()
    {
        Type instanceType = typeof(T);
        MethodInfo? method = instanceType.FindMatchingInstanceMethods("RenderTo", typeof(void), [typeof(TextBuilder)])
            .FirstOrDefault();

        if (method is not null)
        {
            var dynamicMethod = Any.CreateDynamicMethod<AnyRenderTo>($"Any_{instanceType}_RenderTo");
            var gen = dynamicMethod.GetILGenerator();

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Constrained, instanceType);
            gen.Emit(OpCodes.Callvirt, method);
            gen.Emit(OpCodes.Ret);

            if (dynamicMethod.TryCreateDelegate<AnyRenderTo>(out var func))
            {
                Invoke = func;
                return;
            }
        }

        Invoke = Fallback;
    }

    private static void Fallback(in T instance, TextBuilder builder)
    {
        builder.Append(Any.ToString(in instance));
    }
}