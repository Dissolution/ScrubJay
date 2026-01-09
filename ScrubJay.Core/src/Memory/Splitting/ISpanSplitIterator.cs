using ScrubJay.Iteration;

namespace ScrubJay.Memory.Splitting;

[PublicAPI]
public interface ISpanSplitIterator<T> : IIterator<Segment<T>>
{
    SplitOptions Options { get; }
}