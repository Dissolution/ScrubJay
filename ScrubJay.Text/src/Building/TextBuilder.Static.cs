namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    [MustDisposeResource]
    public static TextBuilder Rent() => new TextBuilder();
    
    [MustDisposeResource]
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
        if (statefulBuild is not null)
        {
            using var tb = new TextBuilder();
            statefulBuild(tb, state);
            return tb.ToString();
        }
        return string.Empty;
    }
    

//    public static string Build<R>(Func<TextBuilder, R>? buildFunc)
//#if NET9_0_OR_GREATER
//        where R : allows ref struct
//#endif
//    {
//        return New.Invoke(buildFunc).ToStringAndDispose();
//    }



//    public static string Build<S, R>(S state, Func<TextBuilder, S, R>? buildStateOut)
//    {
//        if (buildStateOut is null)
//            return string.Empty;
//        return New.Invoke(state, buildStateOut).ToStringAndDispose();
//    }

    public static string Build(
        [HandlesResourceDisposal]
        ref InterpolatedTextBuilder interpolatedTextBuilder)
    {
        return interpolatedTextBuilder.ToStringAndDispose();
    }
}