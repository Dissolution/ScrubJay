using System.Reflection;

namespace ScrubJay.Rendering;

[PublicAPI]
public static class RendererCache
{
    private static readonly TypeMap<Delegate> _renderToDelegateCache = [];

    static RendererCache()
    {
        AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(static assembly => assembly.GetTypes())
            .Where(static type => type.IsStatic)
            .SelectMany(static type => type
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .SelectWhere(static method =>
                    IsValidRenderToMethod(method, out var valueType) ? Some((valueType, method)) : None))
            .Consume(static tuple => AddDelegate(tuple.valueType, tuple.method));
    }


    private static void AddDelegate(Type type, MethodInfo method)
    {
        Delegate? del = null;
        try
        {
            Type delegateType = typeof(RenderTo<>).MakeGenericType(type);
            del = Delegate.CreateDelegate(
                delegateType,
                method,
                true);
        }
        catch (Exception ex)
        {
            Debugger.Break();
            Console.WriteLine(ex);
            throw;
        }

        if (del is null)
        {
            Debugger.Break();
            throw Ex.NotImplemented();
        }

        _renderToDelegateCache[type] = del;
    }


    private static void TryRegisterType(Type type)
    {
        // type must be static
        if (!type.IsStatic)
            return;

        // look for methods

        // look for static RenderTo Methods
        type
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .SelectWhere(static method =>
                IsValidRenderToMethod(method, out var valueType) ? Some((valueType, method)) : None)
            .Consume(static tuple => AddDelegate(tuple.valueType, tuple.method));
    }

    private static bool IsValidRenderToMethod(MethodInfo method, [NotNullWhen(true)] out Type? instanceType)
    {
        // for faster return
        instanceType = null;

        // must be [Extension, Renders]
        if (method.GetAttribute<ExtensionAttribute>() is null)
            return false;
        var rendersAttr = method.GetAttribute<RendersAttribute>();
        if (rendersAttr is null)
            return false;

        // must match `public static TextBuilder RenderTo(this T, TextBuilder)`

        // already verified in initial GetMethods
        // if (!method.IsPublic || !method.IsStatic)
        //     return false;
        
        if (method.ReturnType != typeof(TextBuilder))
            return false;
        if (method.Name != "RenderTo")
            return false;

        var parameters = method.GetParameters();
        if (parameters.Length != 2)
            return false;

        var instanceParam = parameters[0];
        if (instanceParam.ParameterType != rendersAttr.Type)
            throw new Exception();

        instanceType = instanceParam.ParameterType;

        if (parameters[1].ParameterType != typeof(TextBuilder))
            return false;

        return true;
    }


    public static string Render<T>(this T? value)
        => TextBuilder.New.Render<T>(value).ToStringAndDispose();

#if NET9_0_OR_GREATER
    public static string Render<T>(this T value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => TextBuilder.New.Render<T>(value, _).ToStringAndDispose();
#endif

    public static string Render<T>(this ReadOnlySpan<T> span)
    {
        return TextBuilder.New.Render<T>(span).ToStringAndDispose();
    }

    public static string Render<T>(this Span<T> span)
    {
        return TextBuilder.New.Render<T>(span).ToStringAndDispose();
    }

    public static TextBuilder Render<T>(this TextBuilder builder, T? value)
    {
        if (value is null)
            return builder.Append("〈null〉");

        if (value is IRenderable)
        {
            return ((IRenderable)value).RenderTo(builder);
        }

        if (_renderToDelegateCache.TryGetValue<T>(out var @delegate))
        {
            if (@delegate is RenderTo<T> renderTo)
            {
                return renderTo(value, builder);
            }
            else
            {
                Debugger.Break();
                throw Ex.NotImplemented();
            }
        }

        return builder.Append<T>(value);
    }

#if NET9_0_OR_GREATER
    public static TextBuilder Render<T>(this TextBuilder builder, T value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
#if DEBUG
        bool isRefStruct = typeof(T).IsByRefLike;
        Debug.Assert(isRefStruct);
#endif

        if (_renderToDelegateCache.TryGetValue<T>(out var @delegate))
        {
            if (@delegate is RenderTo<T> renderTo)
            {
                return renderTo(value, builder);
            }
            else
            {
                Debugger.Break();
                throw Ex.NotImplemented();
            }
        }

        return builder.Append<T>(value, _);
    }
#endif


    public static TextBuilder Render<T>(this TextBuilder builder, ReadOnlySpan<T> span)
    {
        return builder.Append("ReadOnlySpan<")
            .Append(TypeName.For<T>())
            .Append(">[")
            .Delimit(", ", span, static (tb, value) => tb.Render(value))
            .Append(']');
    }

    public static TextBuilder Render<T>(this TextBuilder builder, Span<T> span)
    {
        return builder.Append("Span<")
            .Append(TypeName.For<T>())
            .Append(">[")
            .Delimit(", ", span, static (tb, value) => tb.Render(value))
            .Append(']');
    }
}