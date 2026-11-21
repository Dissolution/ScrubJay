namespace ScrubJay.Text;

public delegate void TextWriteValue<W, T>(W writer, T value)
    where W : ITextWriter, allows ref struct;