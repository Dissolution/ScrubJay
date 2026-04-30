using System.Collections.Concurrent;
using System.Reflection;
using ScrubJay.Text.Debugging;

namespace ScrubJay.Text.Rendering;

internal static class RenderingManager
{
    private static readonly MethodInfo[] _scannedMethods;
    private static readonly ConcurrentDictionary<Type, Action<object, TextBuilder>> _boxedActions = new();

    static RenderingManager()
    {
        _scannedMethods = AppDomain.CurrentDomain
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


    internal static void RenderableRenderTo<R>(R renderable, TextBuilder builder)
        where R : IRenderable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        renderable.RenderTo(builder);
    }

    internal static MethodInfo GetRenderableRenderToMethod(Type renderableType)
    {
        return typeof(RenderingManager)
            .GetMethod(nameof(RenderableRenderTo), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(renderableType);
    }

    internal static void DefaultRenderTo<T>(T value, TextBuilder builder)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        builder.Append<T>(value);
    }


    private static int Specificity(MethodInfo method, Type type)
    {
        if (!method.IsGenericMethodDefinition)
            return 1000;

        var gp = method.GetGenericArguments()[0];
        var attrs = gp.GenericParameterAttributes;
        var score = 200;

        if (attrs.HasFlag(GenericParameterAttributes.Covariant)
            || attrs.HasFlag(GenericParameterAttributes.Contravariant)) score -= 50;
        if (attrs.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint)) score += 10;
        if (attrs.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint)) score += 10;
        if (attrs.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint)) score += 10;
#if NET9_0_OR_GREATER
        if (attrs.HasFlag(GenericParameterAttributes.AllowByRefLike)) score += 10;
#endif
        foreach (var constraint in gp.GetGenericParameterConstraints())
        {
            score += constraint == type ? 110 : constraint.IsAssignableFrom(type) ? 30 : 10;
        }

        return score;
    }

    internal static Option<int> InputConversionSpecificity(Type inputType, Type destinationType)
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
                Debugger.Break();
                return 99;
            }
            return None;
        }

        if (destinationType.IsClass)
        {
            if (destinationType.IsSubclassOf(inputType))
            {
                // subclass is good
                return 75;
            }
        }

        if (destinationType.IsInterface)
        {
            if (inputType.ImplementsInterface(destinationType))
            {
                // interface is fine
                return 50;
            }
        }

        if (destinationType == typeof(object))
        {
            // barely acceptable
            return 1;
        }

        return None;
    }


    internal static Option<(MethodInfo ConcreteMethod, int Specificity)> MatchSpecificity(MethodInfo renderToMethod, Type valueType)
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
            Debug.Assert(genericTypes.Length == 1);

            var t = genericTypes[0];
            var tAttributes = t.GenericParameterAttributes;
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
                    // this is okay
                    concreteMethod = Result.Try(() => renderToMethod.MakeGenericMethod(arrayValueElementType)).OkOrDefault();
                    specificity = 60;
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

    internal static Action<T, TextBuilder>? GetSingletonAction<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var type = typeof(T);
        MethodInfo? renderToMethod = null;

        // Check for IRenderable support
        if (type.GetInterfaces().Any(static f => f == typeof(IRenderable)))
        {
            renderToMethod = GetRenderableRenderToMethod(type);
        }
        else
        {
            // scan for a method that can handle this type
            var matchingMethods = _scannedMethods
                .SelectWhere(method => MatchSpecificity(method, type))
                .OrderByDescending(pair => pair.Specificity)
                .Select(pair => pair.ConcreteMethod)
                .ToList();

            renderToMethod = matchingMethods.FirstOrDefault();
        }

        if (renderToMethod is null)
            return null;

        try
        {
            var delType = typeof(Action<,>).MakeGenericType(type, typeof(TextBuilder));
            var del = Delegate.CreateDelegate(delType, renderToMethod);
            return (Action<T, TextBuilder>)del;
        }
        catch (Exception ex)
        {
            Debug.Log(LogLevel.Info, ex);
            // ignore all issues
            return null;
        }
    }

    internal static Action<object, TextBuilder> GetBoxedAction(Type type)
    {
        return _boxedActions.GetOrAdd(type, static t =>
        {
            if (typeof(IRenderable).IsAssignableFrom(t))
                return static (obj, builder) => ((IRenderable)obj).RenderTo(builder);

            var resolveBoxed = typeof(RenderingManager)
                .GetMethod(nameof(BoxSingletonAction), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(t);

            return (Action<object, TextBuilder>)resolveBoxed.Invoke(null, null)!;
        });
    }

    private static Action<object, TextBuilder> BoxSingletonAction<T>()
    {
        var concreteAction = RendererSingleton<T>.Action;
        if (concreteAction is not null)
        {
            return (obj, builder) => concreteAction((T)obj, builder);
        }
        return DefaultRenderTo<object>;
    }
}