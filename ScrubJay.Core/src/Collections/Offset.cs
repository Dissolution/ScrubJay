namespace ScrubJay.Collections;

/// <summary>
/// Represents an offset in a collection (similar to <see cref="Index"/>)
/// that also includes context around whether the offset is from a collection's
/// logical 'Front' or 'Back'.
/// </summary>
[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 9)]
public readonly struct Offset 
{
    
    
    // >=0 → from start
    // < 0 → from end (encoded as ~value)
    
    /// <remarks>
    /// Encoded the same as <see cref="Index"/>:<br/>
    /// <c>_value &gt;= 0</c>: offset is from Start | Front<br/>
    /// <c>_value &lt; 0</c>: offset is from End | Back<br/>
    /// </remarks>
    [FieldOffset(0)]
    private readonly int _value;

    // true  → pop order (top = 0)
    // false → array order (bottom = 0)
    [FieldOffset(4)]
    private readonly bool _pop;
}