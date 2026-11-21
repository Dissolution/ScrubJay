namespace ScrubJay.Text.Displaying;

[PublicAPI]
public interface IDisplayable
{
    string Display()
    {
        using Interpolator interpolator = new Interpolator();
        this.DisplayTo(interpolator);
        return interpolator.ToString();
    }

    void DisplayTo<W>(W textWriter)
        where W : ITextWriter
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    ;
}