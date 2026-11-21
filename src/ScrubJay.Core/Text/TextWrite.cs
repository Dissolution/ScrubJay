namespace ScrubJay.Text;

public delegate void TextWrite<W>(W writer)
    where W : ITextWriter, allows ref struct;