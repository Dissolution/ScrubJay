namespace ScrubJay.Reflection.Lightweight.Extensions;

[PublicAPI]
public static class ISHExtensions
{
    extension(ref DefaultInterpolatedStringHandler interpolator)
    {
        public void AppendFormatted<T>(in T? value)
            where T : allows ref struct
        {
            interpolator.AppendFormatted(Generic.ToString<T>(in value));
        }
    }
}