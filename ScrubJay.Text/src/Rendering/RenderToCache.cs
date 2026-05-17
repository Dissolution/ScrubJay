using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Text.Collections;

namespace ScrubJay.Text.Rendering;

internal sealed record class RenderToMethodInfo(MethodInfo Method, Type InstanceType, int Priority);

internal static class RenderToCache
{
    private static readonly RenderToMethodInfo[] _renderToMethods;
    private static readonly ConcurrentTypeMap<Action<object, TextBuilder>> _boxedRenderTos = new();

    static RenderToCache()
    {
        _renderToMethods = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(FilterAssembly)
            .SelectMany(GetAssemblyTypes)
            .SelectMany(GetRenderToMethods)
            .OrderByDescending(static method => method.Priority)
            .ToArray();
    }


    private static bool FilterAssembly(Assembly assembly)
    {
        if (assembly.IsDynamic) return false;
        string fullname = assembly.FullName ?? assembly.GetName().FullName;
        if (fullname.StartsWith("Microsoft", StringComparison.Ordinal) ||
            fullname.StartsWith("System", StringComparison.Ordinal))
            return false;
        return true;
    }

    private static IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
    {
        try
        {
            // only public types
            return assembly.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException typeLoadException)
        {
            // the types we could load
            return typeLoadException.Types.WhereNotNull();
        }
        catch
        {
            return Type.EmptyTypes;
        }
    }

    private static IEnumerable<RenderToMethodInfo> GetRenderToMethods(Type type)
    {
        return type
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .SelectWhere<MethodInfo, RenderToMethodInfo>(static method =>
            {
                var renderToMethodAttribute = method.GetCustomAttribute<RenderToMethodAttribute>();
                if (renderToMethodAttribute is null)
                    return None;
                int priority = renderToMethodAttribute.Priority;

                if (method.IsAbstract)
                    return None;
                if (method.ReturnType != typeof(void))
                    return None;
                var methodParameters = method.GetParameters();
                if (methodParameters.Length != 2)
                    return None;
                var valueParameter = methodParameters[0];
                if (valueParameter.IsOut)
                    return None;
                var instanceType = valueParameter.ParameterType;

                var tbParameter = methodParameters[1];
                if (tbParameter.ParameterType != typeof(TextBuilder) ||
                    tbParameter.IsIn || tbParameter.IsOut)
                    return None;

                return new RenderToMethodInfo(method, instanceType, priority);
            });
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
                .GetMethod(nameof(RenderObjectTo), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
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

            }
        }

    }
}

public static class AnyRenderExtensions
{
    extension(Any)
    {
        internal static void RenderNullTo<T>(TextBuilder builder)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            builder
                .Append('(')
                .RenderType<T>()
                .Append(")null");
        }


        public static void RenderTo<T>(in T? instance, TextBuilder builder)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            if (instance is null)
            {
                RenderNullTo<T>(builder);
            }
            else
            {

            }
        }

        public static string Render<T>(in T? instance)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            using var builder = TextBuilder.Rent();
            RenderTo<T>(in instance, builder);
            return builder.ToString();
        }
    }


    private static class RenderToCache<T>
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
}