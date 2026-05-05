namespace ScrubJay.Enums.Extensions;

public struct EnumEnumerator<E> : IEnumerator<E> 
    where E : struct, Enum
{
    object IEnumerator.Current => Current;
    
    public E Current { get; }
    
    public EnumEnumerator(E @enum)
    {
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}