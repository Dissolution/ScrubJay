namespace ScrubJay.Extensions;

[PublicAPI]
public static class EnumeratorExtensions
{
    extension(IEnumerator? enumerator) { }

    extension<E>(E enumerator)
        where E : IEnumerator
    {
        public void Dispose()
        {
            if (enumerator is IDisposable)
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }
}