using ScrubJay.Errors.Validation;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class TupleExtensions
{
    [PublicAPI]
    [MustDisposeResource(false)]
    public struct TupleEnumerator<T> : IEnumerator<object?>, IEnumerator, IDisposable
        where T : ITuple
    {
        private readonly T _tuple;
        private int _index;

        public object? Current => _tuple[_index];

        public TupleEnumerator(T tuple)
        {
            ArgumentNullException.ThrowIfNull(tuple);
            _tuple = tuple;
            _index = -1;
        }

        public bool MoveNext()
        {
            int next = _index + 1;
            if (next < _tuple.Length)
            {
                _index = next;
                return true;
            }
            return false;
        }
        
        public void Reset()
        {
            _index = -1;
        }
        
        void IDisposable.Dispose()
        {
            // do nothing
        }
    }
    
    extension<T>(T tuple)
        where T : ITuple
    {
        public TupleEnumerator<T> GetEnumerator() => new(tuple);
    }
}
