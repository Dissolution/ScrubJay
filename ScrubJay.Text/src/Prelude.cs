namespace ScrubJay.Text;

[PublicAPI]
public static class Prelude
{
    /// <summary>
    /// Renders the interpolated text with a <see cref="InterpolatedTextBuilder"/>.
    /// </summary>
    /// <param name="interpolatedText">
    /// The interpolated text.
    /// </param>
    /// <returns>
    /// The <see cref="string"/> result of interpolation.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string R(
        [HandlesResourceDisposal]
        ref InterpolatedTextBuilder interpolatedText)
    {
        return interpolatedText.ToStringAndDispose();
    }
}