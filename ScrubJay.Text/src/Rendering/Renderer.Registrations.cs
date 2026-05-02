using System.Collections.Concurrent;
using System.Reflection;
using ScrubJay.Text.Collections;
using ScrubJay.Text.Debugging;

namespace ScrubJay.Text.Rendering;

public static partial class Renderer
{
    private static readonly MethodInfo[] _registeredRenderToMethods;
    private static readonly ConcurrentTypeMap<Action<object, TextBuilder>> _boxedActions = new();

    static Renderer()
    {
        _registeredRenderToMethods = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(static assembly =>
            {
                if (assembly.IsDynamic) return false;
                string fullname = assembly.FullName ?? assembly.GetName().FullName;
                if (fullname.StartsWith("Microsoft", StringComparison.Ordinal) ||
                    fullname.StartsWith("System", StringComparison.Ordinal))
                    return false;
                return true;
            })
            .SelectMany(static assembly =>
            {
                try
                {
                    // only public types
                    return assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException typeLoadException)
                {
                    return typeLoadException.Types.WhereNotNull();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .SelectMany(static type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(static method =>
            {
                if (!method.IsDefined(typeof(RenderToMethodAttribute), false))
                    return false;
                if (method.IsAbstract)
                    return false;
                if (method.ReturnType != typeof(void))
                    return false;
                var methodParameters = method.GetParameters();
                if (methodParameters.Length != 2)
                    return false;
                var valueParameter = methodParameters[0];
                if (valueParameter.IsOut)
                    return false;

                var tbParameter = methodParameters[1];
                if (tbParameter.ParameterType != typeof(TextBuilder) ||
                    tbParameter.IsIn || tbParameter.IsOut)
                    return false;

                return true;
            })
            .ToArray();
    }
    
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
                        return (90 - classDepth) - (5 * i);
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

    private static Action<T, TextBuilder>? GetRenderToForCache<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        MethodInfo? renderToMethod = null;

        var type = typeof(T);
        if (type != typeof(object))
        {
            // scan for a method that can handle this type
            var matchingMethods = _registeredRenderToMethods
                .SelectWhere(method => MatchSpecificity(method, type))
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
        else
        {
            renderToMethod = typeof(Renderer)
                .GetMethod(nameof(RenderObjectTo), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!;
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

    private static Action<object, TextBuilder> GetObjectRenderTo(Type type)
    {
        return _boxedActions.GetOrAdd(type, static t =>
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
        var concreteAction = Cache<T>.Action;
        if (concreteAction is not null)
        {
            return (obj, builder) => concreteAction((T)obj, builder);
        }
        return DefaultRenderTo<object>;
    }
    
    
    
    public static void Print()
    {
        var msg = TextBuilder.Rent()
            .Append($"Registered {_registeredRenderToMethods.Length} RenderToMethods:")
            .NewLine()
            .Delimit(TB.NewLine, _registeredRenderToMethods, TB.Render)
            .ToStringAndDispose();
        Trouble.Info(msg);
        Trouble.Hold();
    }
}