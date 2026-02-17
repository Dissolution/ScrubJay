namespace ScrubJay.Rendering.Rendition3;

public delegate TextBuilder RenderTo<in T>(T value, TextBuilder builder)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;