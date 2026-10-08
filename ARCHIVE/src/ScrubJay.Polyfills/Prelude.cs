using ScrubJay.Polyfills.Text;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class Prelude
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string I([HandlesResourceDisposal] ref InterpolatedText interpolatedText)
    {
        return interpolatedText.ToStringAndDispose();
    }
}