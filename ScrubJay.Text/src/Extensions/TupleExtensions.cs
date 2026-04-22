namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class TupleExtensions
{
    internal sealed class TupleIterator<T>
        where T : ITuple
    {
        private readonly T _tuple;
        private int _index;

        public TupleIterator(T tuple)
        {
            _tuple = tuple;
            _index = -1;
        }

        public Option<object?> Next()
        {
            int nextIndex = _index + 1;
            if (nextIndex < _tuple.Length)
            {
                _index = nextIndex;
                return Some<object?>(_tuple[nextIndex]);
            }
            return None;
        }
    }
    
    extension<T>(T? tuple)
        where T : ITuple
    {
        [return: NotNullIfNotNull(nameof(tuple))]
        public Func<Option<object?>> GetIterator()
        {
            if (tuple is not null)
            {
                return new TupleIterator<T>(tuple).Next;
            }
            return static () => None;
        }
    }
}
