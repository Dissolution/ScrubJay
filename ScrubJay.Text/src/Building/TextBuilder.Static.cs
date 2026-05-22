namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public static TextBuilder Rent() => new TextBuilder();

    public static TextBuilder Rent(int minCapacity) => new TextBuilder(minCapacity);


    public static string Build(Action<TextBuilder>? build)
    {
        if (build is not null)
        {
            using var tb = new TextBuilder();
            build(tb);
            return tb.ToString();
        }
        return string.Empty;
    }

    public static string Build(
        [HandlesResourceDisposal] ref InterpolatedTextBuilder interpolatedTextBuilder)
    {
        return interpolatedTextBuilder.ToStringAndDispose();
    }

#region Build with State
    public static string Build<T>(T instance, Action<T, TextBuilder>? instanceBuild)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instanceBuild is null)
            return string.Empty;

        var builder = Rent();
        instanceBuild(instance, builder);
        return builder.ToStringAndDispose();
    }

    public static string Build<T>(Action<TextBuilder, T>? instanceBuild, T instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instanceBuild is null)
            return string.Empty;

        var builder = Rent();
        instanceBuild(builder, instance);
        return builder.ToStringAndDispose();
    }


    public static string Build<T>(in T instance, InValueTextBuild<T>? instanceBuild)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instanceBuild is null)
            return string.Empty;

        var builder = Rent();
        instanceBuild(in instance, builder);
        return builder.ToStringAndDispose();
    }

    public static string Build<T>(TextBuildWithInValue<T>? instanceBuild, in T instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instanceBuild is null)
            return string.Empty;

        var builder = Rent();
        instanceBuild(builder, in instance);
        return builder.ToStringAndDispose();
    }


    public static string Build<T>(scoped ReadOnlySpan<T> span, ReadOnlySpanTextBuild<T>? spanBuild)
    {
        if (spanBuild is null)
            return string.Empty;

        var builder = Rent();
        spanBuild(span, builder);
        return builder.ToStringAndDispose();
    }

    public static string Build<T>(TextBuildWithReadOnlySpan<T>? instanceBuild, scoped ReadOnlySpan<T> span)
    {
        if (instanceBuild is null)
            return string.Empty;

        var builder = Rent();
        instanceBuild(builder, span);
        return builder.ToStringAndDispose();
    }
#endregion
}