using System.ComponentModel;

namespace ScrubJay.Text.Utilities;

[PublicAPI]
public ref struct TextSplitEnumerator :
    IEnumerator<Range>,
    IEnumerator,
    IDisposable
{
    private readonly text _source;

    /// <summary>A single separator to use when <see cref="_splitMode"/> is <see cref="TextSplitEnumeratorMode.SingleElement"/>.</summary>
    private readonly char _separator = default!;
    /// <summary>
    /// A separator span to use when <see cref="_splitMode"/> is <see cref="TextSplitEnumeratorMode.Sequence"/> (in which case
    /// it's treated as a single separator) or <see cref="TextSplitEnumeratorMode.Any"/> (in which case it's treated as a set of separators).
    /// </summary>
    private readonly text _separatorBuffer;
#if NET8_0_OR_GREATER
    private static readonly SearchValues<char> _whitespaceSearchChars = SearchValues.Create("\t\n\v\f\r\u0020\u0085\u00a0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000");

    private readonly SearchValues<char> _searchValues = default!;
#endif

    /// <summary>Mode that dictates how the instance was configured and how its fields should be used in <see cref="MoveNext"/>.</summary>
    private TextSplitEnumeratorMode _splitMode;
    /// <summary>The inclusive starting index in <see cref="_source"/> of the current range.</summary>
    private int _startCurrent = 0;
    /// <summary>The exclusive ending index in <see cref="_source"/> of the current range.</summary>
    private int _endCurrent = 0;
    /// <summary>The index in <see cref="_source"/> from which the next separator search should start.</summary>
    private int _startNext = 0;

    /// <summary>Gets an enumerator that allows for iteration over the split span.</summary>
    /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/> that can be used to iterate over the split span.</returns>
    public TextSplitEnumerator GetEnumerator() => this;

    /// <summary>Gets the source span being enumerated.</summary>
    /// <returns>Returns the <see cref="ReadOnlySpan{T}"/> that was provided when creating this enumerator.</returns>
    public readonly text Source => _source;


    public Range CurrentRange => new Range(_startCurrent, _endCurrent);

    public text CurrentText => _source.Slice(_startCurrent, _endCurrent - _startCurrent);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Range Current => CurrentRange;

#if NET8_0_OR_GREATER
    internal TextSplitEnumerator(text source, SearchValues<char> searchValues)
    {
        _source = source;
        _splitMode = TextSplitEnumeratorMode.SearchValues;
        _searchValues = searchValues;
    }
#endif

    /// <summary>Initializes the enumerator for <see cref="TextSplitEnumeratorMode.Any"/>.</summary>
    /// <remarks>
    /// If <paramref name="separators"/> is empty, as an optimization
    /// it will instead use <see cref="TextSplitEnumeratorMode.SearchValues"/> with a cached <see cref="SearchValues{Char}"/>
    /// for all whitespace characters.
    /// </remarks>
    internal TextSplitEnumerator(text source, text separators)
    {
        _source = source;
        if (separators.Length == 0)
        {
            _searchValues = _whitespaceSearchChars;
            _splitMode = TextSplitEnumeratorMode.SearchValues;
        }
        else
        {
            _separatorBuffer = separators;
            _splitMode = TextSplitEnumeratorMode.Any;
        }
    }

    /// <summary>Initializes the enumerator for <see cref="TextSplitEnumeratorMode.Sequence"/> (or <see cref="TextSplitEnumeratorMode.EmptySequence"/> if the separator is empty).</summary>
    /// <remarks><paramref name="treatAsSingleSeparator"/> must be true.</remarks>
    internal TextSplitEnumerator(text source, text separator, bool treatAsSingleSeparator)
    {
        Debug.Assert(treatAsSingleSeparator, "Should only ever be called as true; exists to differentiate from separators overload");

        _source = source;
        _separatorBuffer = separator;
        _splitMode = separator.Length == 0 ? TextSplitEnumeratorMode.EmptySequence : TextSplitEnumeratorMode.Sequence;
    }

    /// <summary>Initializes the enumerator for <see cref="TextSplitEnumeratorMode.SingleElement"/>.</summary>
    internal TextSplitEnumerator(text source, char separator)
    {
        _source = source;
        _separator = separator;
        _splitMode = TextSplitEnumeratorMode.SingleElement;
    }

    /// <summary>
    /// Advances the enumerator to the next element of the enumeration.
    /// </summary>
    /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the enumeration.</returns>
    public bool MoveNext()
    {
        // Search for the next separator index.
        int separatorIndex, separatorLength;
        switch (_splitMode)
        {
            case TextSplitEnumeratorMode.None:
                return false;

            case TextSplitEnumeratorMode.SingleElement:
                separatorIndex = _source.Slice(_startNext).IndexOf(_separator);
                separatorLength = 1;
                break;

            case TextSplitEnumeratorMode.Any:
                separatorIndex = _source.Slice(_startNext).IndexOfAny(_separatorBuffer);
                separatorLength = 1;
                break;

            case TextSplitEnumeratorMode.Sequence:
                separatorIndex = _source.Slice(_startNext).IndexOf(_separatorBuffer);
                separatorLength = _separatorBuffer.Length;
                break;

            case TextSplitEnumeratorMode.EmptySequence:
                separatorIndex = -1;
                separatorLength = 1;
                break;

            default:
                Debug.Assert(_splitMode == TextSplitEnumeratorMode.SearchValues, $"Unknown split mode: {_splitMode}");
                separatorIndex = _source.Slice(_startNext).IndexOfAny(_searchValues);
                separatorLength = 1;
                break;
        }

        _startCurrent = _startNext;
        if (separatorIndex >= 0)
        {
            _endCurrent = _startCurrent + separatorIndex;
            _startNext = _endCurrent + separatorLength;
        }
        else
        {
            _startNext = _endCurrent = _source.Length;

            // Set _splitMode to None so that subsequent MoveNext calls will return false.
            _splitMode = TextSplitEnumeratorMode.None;
        }

        return true;
    }

    /// <inheritdoc />
    object IEnumerator.Current => CurrentRange;

    /// <inheritdoc />
    void IEnumerator.Reset() => throw new NotSupportedException();

    /// <inheritdoc />
    void IDisposable.Dispose() { }
}

/// <summary>Indicates in which mode <see cref="TextSplitEnumerator"/> is operating, with regards to how it should interpret its state.</summary>
internal enum TextSplitEnumeratorMode
{
    /// <summary>Either a default <see cref="TextSplitEnumerator"/> was used, or the enumerator has finished enumerating and there's no more work to do.</summary>
    None = 0,

    /// <summary>A single T separator was provided.</summary>
    SingleElement,

    /// <summary>A span of separators was provided, each of which should be treated independently.</summary>
    Any,

    /// <summary>The separator is a span of elements to be treated as a single sequence.</summary>
    Sequence,

    /// <summary>The separator is an empty sequence, such that no splits should be performed.</summary>
    EmptySequence,

#if NET8_0_OR_GREATER
    /// <summary>
    /// A <see cref="SearchValues{Char}"/> was provided and should behave the same as with <see cref="Any"/> but with the separators in the <see cref="SearchValues"/>
    /// instance instead of in a <see cref="ReadOnlySpan{Char}"/>.
    /// </summary>
    SearchValues,
#endif
}