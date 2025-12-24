namespace ScrubJay.Text;

[PublicAPI]
public static class Prelude
{
    public static string Build(
        [HandlesResourceDisposal]
        InterpolatedTextHandler handler)
    {
        return handler.ToStringAndDispose();
    }
}