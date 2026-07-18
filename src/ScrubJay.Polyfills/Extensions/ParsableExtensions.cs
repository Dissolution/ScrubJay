#if NET7_0_OR_GREATER
namespace ScrubJay.Polyfills;

[PublicAPI]
public static class ParsableExtensions
{
    extension<P>(P)
        where P : IParsable<P>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            [NotNullWhen(true)] string? str, [MaybeNullWhen(returnValue: false)] out P parsed)
            => P.TryParse(str, null, out parsed);
    }

    extension<P>(P)
        where P : ISpanParsable<P>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            text text, [MaybeNullWhen(returnValue: false)] out P parsed)
            => P.TryParse(text, null, out parsed);
    }
}

#endif