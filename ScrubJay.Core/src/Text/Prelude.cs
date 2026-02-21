namespace ScrubJay.Text;

[PublicAPI]
public static class Prelude
{
    public static string Build(
        [HandlesResourceDisposal]
        ref InterpolatedTextBuilder handler)
    {
        return handler.ToStringAndDispose();
    }
}