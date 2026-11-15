namespace ScrubJay.Text.Displaying;

[PublicAPI]
public interface IDisplayable
{
    string Display();

    void DisplayTo<W>(ref W textWriter)
        where W : ITextWriter;
}