//using System.Reflection;
//
//namespace ScrubJay.Rendering.Rendition5;
//
//[PublicAPI]
//public static class RendererCache
//{
//    private static readonly List<MethodInfo> _rendererMethods = [];
//    private static readonly ConcurrentTypeMap<Delegate?> _renderToDelegates = [];
//
//    static RendererCache()
//    {
//        AppDomain
//            .CurrentDomain
//            .GetAssemblies()
//            .Where(static assembly =>
//            {
//                // skip system assemblies
//                if (assembly.FullName is not null &&
//                    (assembly.FullName.StartsWith("System", StringComparison.Ordinal) ||
//                        assembly.FullName.StartsWith("Microsoft", StringComparison.Ordinal)))
//                {
//                    return false;
//                }
//
//                // dynamic assemblies are prone to throwing exceptions
//                if (assembly.IsDynamic)
//                    return false;
//                return true;
//            })
//            .SelectMany(static assembly => assembly.GetExportedTypes())
//            .SelectMany(static type =>
//                type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
//            .Where(static method =>
//            {
//                if (!method.IsDefined(typeof(RenderToMethodAttribute), false))
//                    return false;
//                if (method.ReturnType != typeof(void))
//                    return false;
//                var methodParams = method.GetParameters();
//                if (methodParams.Length != 2)
//                    return false;
//                var builderParam = methodParams[1];
//                if (builderParam.ParameterType != typeof(TextBuilder) || builderParam.IsIn || builderParam.IsOut)
//                    return false;
//                return true;
//            })
//            .Consume(static method =>
//            {
//                // if this is generic, we have to cache it until we have a concrete type
//                if (method.IsGenericMethodDefinition)
//                {
//                    _rendererMethods.Add(method);
//                }
//                else
//                {
//                    // otherwise build + register that concrete delegate now
//                    var paramType = method.GetParameters()[0].ParameterType;
//                    var delegateType = typeof(RenderTo<>).MakeGenericType(paramType);
//                    var del = Delegate.CreateDelegate(delegateType, method);
//                    _renderToDelegates.TryAdd(paramType, del);
//                }
//            });
//    }
//
//    private static bool TryGetRenderer<T>([NotNullWhen(true)] out RenderTo<T>? renderer)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var del = _renderToDelegates.GetOrAdd<T>(_ => FindOrCreateRenderTo<T>());
//        return del.Is<RenderTo<T>>(out renderer);
//    }
//
//    private static RenderTo<T>? FindOrCreateRenderTo<T>()
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        // scan our renderer methods
//        var match = _rendererMethods
//            .SelectWhere(static method =>
//            {
//                try
//                {
//                    // if this can be made, then T passes all the type constraints
//                    var concreteMethod = method.MakeGenericMethod(typeof(T));
//                    return Some((method, concreteMethod));
//                }
//                catch
//                {
//                    return None;
//                }
//            })
//            // we want the most specific match
//            .OrderByDescending(static tuple => Specificity(tuple.method, typeof(T)))
//            .FirstOrDefault();
//
//        if (match.concreteMethod is not null)
//        {
//            // now we can generate a delegate to cache since we have a concrete instance type
//            return Delegate.CreateDelegate<RenderTo<T>>(match.concreteMethod);
//        }
//
//        // we do not have any way to deal with this
//        return null;
//    }
//
//    private static int Specificity(MethodInfo method, Type parameterType)
//    {
//        // non-generic methods are very specific
//        if (!method.IsGeneric)
//        {
//            // but subtypes are less specific
//            var methodParameterType = method.GetParameters()[0].ParameterType;
//            if (methodParameterType == parameterType)
//            {
//                // exact match
//                return 1000;
//            }
//            else if (methodParameterType.IsSubclassOf(parameterType))
//            {
//                // subclass is fairly specific
//                return 900;
//            }
//            else if (methodParameterType.IsInterface)
//            {
//                // interface is less specific
//                return 800;
//            }
//            else
//            {
//                Debugger.Break();
//                return 700;
//            }
//        }
//
//        // generic methods are less specific
//        int score = 200;
//
//        var genericParams = method.GetGenericArguments();
//        if (genericParams.Length == 0)
//            return score;
//
//        var genericParam = genericParams[0];
//
//        // we can use constraints on the generic type to define the specificity
//        var attrs = genericParam.GenericParameterAttributes;
//        if (attrs.HasAnyFlags(GenericParameterAttributes.Covariant, GenericParameterAttributes.Contravariant))
//        {
//            // out and in are less specific
//            score -= 50;
//        }
//
//        // other common constraints each add to specificity
//        if (attrs.HasFlags(GenericParameterAttributes.ReferenceTypeConstraint))
//            score += 10;
//        if (attrs.HasFlags(GenericParameterAttributes.NotNullableValueTypeConstraint))
//            score += 10;
//        if (attrs.HasFlags(GenericParameterAttributes.DefaultConstructorConstraint))
//            score += 10;
//#if NET9_0_OR_GREATER
//        if (attrs.HasFlags(GenericParameterAttributes.AllowByRefLike))
//            score += 10;
//#endif
//
//        // same for generic parameter constraints
//        // Count type constraints (interfaces, base classes, Enum, etc.)
//        score += (genericParam.GetGenericParameterConstraints().Length * 10);
//
//        return score;
//    }
//
//    /// <summary>
//    /// Render this <typeparamref name="T"/> <paramref name="value"/> to a <see cref="TextBuilder"/>.
//    /// </summary>
//    /// <param name="value">
//    /// The <typeparamref name="T"/> value to render
//    /// </param>
//    /// <param name="builder">
//    /// The <see cref="TextBuilder"/> to render to
//    /// </param>
//    /// <typeparam name="T">
//    /// The <see cref="Type"/> of value to be rendered.<br/>
//    /// <b>Note:</b> in NET9.0+, this even allows for <c>ref struct</c> values.
//    /// </typeparam>
//    public static void RenderTo<T>(this T? value, TextBuilder builder)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (value is null)
//        {
//            builder.Write("〈null〉");
//        }
//        else
//        {
//            if (TryGetRenderer<T>(out var renderer))
//            {
//                renderer(value, builder);
//            }
//            else
//            {
//                builder.Append<T>(value);
//            }
//        }
//    }
//
//    /// <summary>
//    /// Renders a <typeparamref name="T"/> <paramref name="value"/> to this <see cref="TextBuilder"/>.
//    /// </summary>
//    /// <param name="builder">
//    /// The <see cref="TextBuilder"/> to render to
//    /// </param>
//    /// <param name="value">
//    /// The <typeparamref name="T"/> value to render
//    /// </param>
//    /// <typeparam name="T">
//    /// The <see cref="Type"/> of value to be rendered.<br/>
//    /// <b>Note:</b> in NET9.0+, this even allows for <c>ref struct</c> values.
//    /// </typeparam>
//    /// <returns>
//    /// The <see cref="TextBuilder"/> after rendering the value.
//    /// </returns>
//    public static TextBuilder Render<T>(this TextBuilder builder, T? value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (value is null)
//        {
//            return builder.Append("〈null〉");
//        }
//
//        if (TryGetRenderer<T>(out var renderer))
//        {
//            renderer(value, builder);
//            return builder;
//        }
//
//        return builder.Append<T>(value);
//    }
//
//    /// <summary>
//    /// Render this <typeparamref name="T"/> <paramref name="value"/>.
//    /// </summary>
//    /// <param name="value">
//    /// The <typeparamref name="T"/> value to render
//    /// </param>
//    /// <typeparam name="T">
//    /// The <see cref="Type"/> of value to be rendered.<br/>
//    /// <b>Note:</b> in NET9.0+, this even allows for <c>ref struct</c> values.
//    /// </typeparam>
//    /// <returns>
//    /// The <see cref="string"/> rendering.
//    /// </returns>
//    public static string Render<T>(this T? value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        using var builder = TextBuilder.New;
//        value.RenderTo<T>(builder);
//        return builder.ToString();
//    }
//
//    public static string Render<T>(this ReadOnlySpan<T> span)
//    {
//        return TextBuilder.New
//            .Append('[')
//            .Delimit(", ", span, TBA<T>.Render)
//            .Append(']')
//            .ToStringAndDispose();
//
//    }
//    
//    public static string Render<T>(this Span<T> span)
//    {
//        return TextBuilder.New
//            .Append('[')
//            .Delimit(", ", span, TBA<T>.Render)
//            .Append(']')
//            .ToStringAndDispose();
//
//    }
//}