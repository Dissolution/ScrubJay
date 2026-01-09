namespace ScrubJay.Text;

[PublicAPI]
public static class Prelude
{
    public static string Build(
        [HandlesResourceDisposal]
        InterpolatedTextBuilder handler)
    {
        return handler.ToStringAndClear();
    }
}