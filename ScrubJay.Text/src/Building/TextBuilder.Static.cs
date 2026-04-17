namespace ScrubJay.Text.Building;

public ref partial struct TextBuilder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextBuilder Create() => new TextBuilder();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextBuilder Create(int minCapacity) => new TextBuilder(minCapacity);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextBuilder Create(Span<char> initialBuffer) => new TextBuilder(initialBuffer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialBuffer"></param>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="startPosition"/> is not valid for the <paramref name="initialBuffer"/>.
    /// </exception>
    public static TextBuilder Create(Span<char> initialBuffer, int startPosition)
    {
        if ((uint)startPosition < (uint)initialBuffer.Length)
        {
            return new TextBuilder(initialBuffer, startPosition);
        }
        throw new ArgumentOutOfRangeException(nameof(startPosition), startPosition, $"Was not inside initial buffer char[{initialBuffer.Length}]");
    }

//
//    public static string Build(Action<TextBuilder>? build)
//    {
//        return New.Invoke(build).ToStringAndDispose();
//    }
//
//    public static string Build<R>(Func<TextBuilder, R>? buildOut)
//#if NET9_0_OR_GREATER
//        where R : allows ref struct
//#endif
//    {
//        return New.Invoke(buildOut).ToStringAndDispose();
//    }
//
//    public static string Build<S>(S state, Action<TextBuilder, S>? buildState)
//#if NET9_0_OR_GREATER
//        where S : allows ref struct
//#endif
//    {
//        return New.Invoke(state, buildState).ToStringAndDispose();
//    }
//
//    public static string Build<S, R>(S state, Func<TextBuilder, S, R>? buildStateOut)
//    {
//        if (buildStateOut is null)
//            return string.Empty;
//        return New.Invoke(state, buildStateOut).ToStringAndDispose();
//    }
//
//    public static string Build(ref InterpolatedTextBuilder interpolatedTextBuilder)
//    {
//        return interpolatedTextBuilder.ToStringAndDispose();
//    }
}