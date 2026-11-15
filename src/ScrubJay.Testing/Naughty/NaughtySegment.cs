using System.Buffers;

namespace ScrubJay.Testing.Naughty;

/// <summary>
/// Builds multi-segment <see cref="ReadOnlySequence{T}"/> values, which the framework has no public factory for.
/// </summary>
public sealed class NaughtySegment<T> : ReadOnlySequenceSegment<T>
{
    public NaughtySegment(ReadOnlyMemory<T> memory)
    {
        Memory = memory;
    }

    public static ReadOnlySequence<T> Sequence(params ReadOnlyMemory<T>[] parts)
    {
        var first = new NaughtySegment<T>(parts[0]);
        var last = first;
        foreach (var part in parts.Skip(1))
        {
            last = last.Append(part);
        }

        return new ReadOnlySequence<T>(first, 0, last, last.Memory.Length);
    }

    private NaughtySegment<T> Append(ReadOnlyMemory<T> memory)
    {
        var next = new NaughtySegment<T>(memory) { RunningIndex = RunningIndex + Memory.Length };
        Next = next;
        return next;
    }
}