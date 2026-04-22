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
    
    public static string Build<S>(S state, Action<TextBuilder, S>? statefulBuild)
#if NET9_0_OR_GREATER
        where S : allows ref struct
#endif
    {
        using var tb = new TextBuilder();
        if (statefulBuild is not null)
        {
            statefulBuild(tb, state);
        }
        else
        {
            tb.Write<S>(state);
        }
        return tb.ToString();
    }
    
    public static string Build(
        [HandlesResourceDisposal]
        ref InterpolatedTextBuilder interpolatedTextBuilder)
    {
        return interpolatedTextBuilder.ToStringAndDispose();
    }
}