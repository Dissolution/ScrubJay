namespace ScrubJay.Iteration;

// https://github.com/Cysharp/ZLinq/tree/main

[PublicAPI]
public interface IIterator<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    bool TryMoveNext([MaybeNullWhen(false)] out T value);
}