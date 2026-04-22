using System.Collections.Concurrent;
using System.Reflection;
using ScrubJay.Universal.Extensions;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class RendererCache
{
    private static readonly ConcurrentDictionary<string, byte> _processedAssemblies = [];
    private static readonly ConcurrentBag<MethodInfo> _rendererMethods = [];
    private static readonly ConcurrentDictionary<Type, Delegate?> _renderToDelegates = [];

    private static void LoadTypeRenderToMethods(Type type)
    {
        // get our public static methods (but only the ones on this type)
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        // check for methods with the attribute that match the signature
        foreach (var method in methods)
        {
            if (!method.IsDefined(typeof(RenderToMethodAttribute), false))
                continue;
            if (method.ReturnType != typeof(void))
                continue;
            var methodParams = method.GetParameters();
            if (methodParams.Length != 2)
                continue;
            var builderParam = methodParams[1];
            if (builderParam.ParameterType != typeof(TextBuilder) || builderParam.IsIn || builderParam.IsOut)
                continue;

            // if this is generic, we have to cache it until we have a concrete type
            if (method.IsGenericMethodDefinition)
            {
                _rendererMethods.Add(method);
            }
            else
            {
                // otherwise build + register that concrete delegate now
                try
                {
                    var paramType = method.GetParameters()[0].ParameterType;
                    var delegateType = typeof(RenderTo<>).MakeGenericType(paramType);
                    var del = Delegate.CreateDelegate(delegateType, method);
                    _renderToDelegates.TryAdd(paramType, del);
                }
                catch
                {
                    // things can often go wrong, do not cause issues for any consuming libraries
                }
            }
        }
    }

    private static void LoadAssemblyRenderToMethods(Assembly assembly)
    {
        // skip anything we've already processed
        if (!_processedAssemblies.TryAdd(assembly.GetName().FullName, 0))
            return;
        
        // skip System and Microsoft assemblies
        if (assembly.FullName is not null &&
            (assembly.FullName.StartsWith("System", StringComparison.Ordinal) || assembly.FullName.StartsWith("Microsoft", StringComparison.Ordinal)))
        {
            return;
        }
        
        // dynamic assemblies are prone to throwing exceptions
        if (assembly.IsDynamic)
            return;

        // get all exported types (we only look for truly public RenderToMethods)
        var exportedTypes = assembly.GetExportedTypes();
        // scan their methods + register
        foreach (var type in exportedTypes)
        {
            LoadTypeRenderToMethods(type);
        }
    }

    static RendererCache()
    {
        var domain = AppDomain.CurrentDomain;
        domain
            .GetAssemblies()
            .Consume(assembly => LoadAssemblyRenderToMethods(assembly));
        domain.AssemblyLoad += (_, args) => LoadAssemblyRenderToMethods(args.LoadedAssembly);
    }

    internal static bool TryGetRenderer<T>([NotNullWhen(true)] out RenderTo<T>? renderer)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var del = _renderToDelegates.GetOrAdd(typeof(T), _ => FindOrCreateRenderTo<T>());
        return del.Is<RenderTo<T>>(out renderer);
    }

    private static RenderTo<T>? FindOrCreateRenderTo<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        // scan our renderer methods
        var match = _rendererMethods
            .SelectWhere(static method =>
            {
                try
                {
                    // if this can be made, then T passes all the type constraints
                    var concreteMethod = method.MakeGenericMethod(typeof(T));
                    return Some((method, concreteMethod));
                }
                catch
                {
                    return None;
                }
            })
            // we want the most specific match
            .OrderByDescending(static tuple => Specificity(tuple.method, typeof(T)))
            .FirstOrDefault();

        if (match.concreteMethod is not null)
        {
            // now we can generate a delegate to cache since we have a concrete instance type
            return (RenderTo<T>)Delegate.CreateDelegate(typeof(RenderTo<T>), match.concreteMethod);
        }

        // we do not have any way to deal with this
        return null;
    }

    private static int Specificity(MethodInfo method, Type parameterType)
    {
        // non-generic methods are very specific
        if (!method.IsGenericMethod)
        {
            // but subtypes are less specific
            var methodParameterType = method.GetParameters()[0].ParameterType;
            if (methodParameterType == parameterType)
            {
                // exact match
                return 1000;
            }
            else if (methodParameterType.IsSubclassOf(parameterType))
            {
                // subclass is fairly specific
                return 900;
            }
            else if (methodParameterType.IsInterface)
            {
                // interface is less specific
                return 800;
            }
            else
            {
                Debugger.Break();
                return 700;
            }
        }

        // generic methods are less specific
        int score = 200;

        var genericParams = method.GetGenericArguments();
        if (genericParams.Length == 0)
            return score;

        var genericParam = genericParams[0];

        // we can use constraints on the generic type to define the specificity
        var attrs = genericParam.GenericParameterAttributes;
        if (attrs.HasFlag(GenericParameterAttributes.Covariant) || attrs.HasFlag(GenericParameterAttributes.Contravariant))
        {
            // out and in are less specific
            score -= 50;
        }

        // other common constraints each add to specificity
        if (attrs.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
            score += 10;
        if (attrs.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
            score += 10;
        if (attrs.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
            score += 10;
#if NET9_0_OR_GREATER
        if (attrs.HasFlag(GenericParameterAttributes.AllowByRefLike))
            score += 10;
#endif

        // same for generic parameter constraints
        // Count type constraints (interfaces, base classes, Enum, etc.)
        score += (genericParam.GetGenericParameterConstraints().Length * 10);

        return score;
    }
    
  


}