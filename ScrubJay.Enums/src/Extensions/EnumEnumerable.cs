namespace ScrubJay.Enums.Extensions;

public struct EnumEnumerable<E> : IEnumerable<E>
    where E : struct, Enum
{
    public EnumEnumerable(E @enum)
    {
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<E> IEnumerable<E>.GetEnumerator() => GetEnumerator();

    public EnumEnumerator<E> GetEnumerator()
    {
        throw new NotImplementedException();
    }
}