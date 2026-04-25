using ScrubJay.Text.Comparison;

namespace ScrubJay.Text.Utilities;

[PublicAPI]
public ref struct SplitTextEnumerator : 
    IEnumerator<Range>,
#if NET9_0_OR_GREATER
    IEnumerator<text>,
#endif
    IEnumerator, IDisposable
{
    private enum SplitMode
    {
        /// <summary>Either a default <see cref="SplitTextEnumerator"/> was used, or the enumerator has finished enumerating and there's no more work to do.</summary>
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


    // SingleElement separator character
    private readonly char _separator = default;

    // Sequence => single separator
    // Any => set of separators
    private readonly text _separatorBuffer = default;

#if NET8_0_OR_GREATER
    private readonly SearchValues<char>? _searchValues = default;
#endif

    private readonly TextComparison _comparison = TextComparison.Ordinal;

    public readonly text Source;

    // enumerator variables

    private SplitMode _splitMode;
    private int _scanStartIndex = 0;
    private int _currentStartIndex = 0;
    private int _currentAfterEndIndex = 0;

    public Range CurrentRange => new(_currentStartIndex, _currentAfterEndIndex);

    public text CurrentText => Source.Slice(_currentStartIndex, _currentAfterEndIndex - _currentStartIndex);


    object? IEnumerator.Current => CurrentRange;

    Range IEnumerator<Range>.Current => CurrentRange;

#if NET9_0_OR_GREATER
    text IEnumerator<text>.Current => CurrentText;
#endif


#region Constructors
    internal SplitTextEnumerator(text source, char separator, TextComparison? comparison = null)
    {
        Source = source;
        _separator = separator;
        _splitMode = SplitMode.SingleElement;
        _comparison = comparison ?? TextComparison.Ordinal;
    }

    internal SplitTextEnumerator(text source, text separator, bool treatAsSingleSeparator, TextComparison? comparison = null)
    {
        Source = source;
        _separatorBuffer = separator;
        if (treatAsSingleSeparator)
        {
            _splitMode = separator.Length == 0 ? SplitMode.EmptySequence : SplitMode.Sequence;
        }
        else
        {
            _splitMode = separator.Length == 0 ? SplitMode.EmptySequence : SplitMode.Any;
        }
        _comparison = comparison ?? TextComparison.Ordinal;
    }

#if NET8_0_OR_GREATER
    internal SplitTextEnumerator(text source, SearchValues<char> searchValues, TextComparison? comparison = null)
    {
        Source = source;
        _splitMode = SplitMode.SearchValues;
        _searchValues = searchValues;
        _comparison = comparison ?? TextComparison.Ordinal;
    }
#endif
#endregion

    /// <summary>
    /// Advances the enumerator to the next element of the enumeration.
    /// </summary>
    /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the enumeration.</returns>
    public bool MoveNext()
    {
        // Search for the next separator index.
        int separatorIndex;
        int separatorLength = 1;
        switch (_splitMode)
        {
            case SplitMode.SingleElement:
            {
                separatorIndex = Source
                    .Slice(_scanStartIndex)
                    .FindIndexOf(_separator, comparison: _comparison)
                    .SomeOr(-1);
                break;
            }
            case SplitMode.Any:
            {
                separatorIndex = Source
                    .Slice(_scanStartIndex)
                    .FindIndexOf(_separatorBuffer, comparison: _comparison)
                    .SomeOr(-1);
                break;
            }
            case SplitMode.Sequence:
            {
                separatorIndex = Source
                    .Slice(_scanStartIndex)
                    .FindIndexOf(_separatorBuffer, comparison: _comparison)
                    .SomeOr(-1);
                separatorLength = _separatorBuffer.Length;
                break;
            }
#if NET8_0_OR_GREATER
            case SplitMode.SearchValues:
            {
                separatorIndex = Source
                    .Slice(_scanStartIndex)
                    .IndexOfAny(_searchValues!);
                break;
            }
#endif
            case SplitMode.EmptySequence:
            {
                separatorIndex = -1;
                break;
            }
            case SplitMode.None:
            default:
                return false;
        }

        _currentStartIndex = _scanStartIndex;
        if (separatorIndex >= 0)
        {
            _currentAfterEndIndex = _currentStartIndex + separatorIndex;
            _scanStartIndex = _currentAfterEndIndex + separatorLength;
        }
        else
        {
            _scanStartIndex = _currentAfterEndIndex = Source.Length;

            // Set _splitMode to None so that subsequent MoveNext calls will return false.
            _splitMode = SplitMode.None;
        }

        return true;
    }

    void IEnumerator.Reset() => throw new NotSupportedException($"{nameof(SplitTextEnumerator)} cannot be Reset");

    void IDisposable.Dispose() { } // do nothing
}