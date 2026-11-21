#pragma warning disable MA0102, IDE0251

namespace ScrubJay.Collections;

[PublicAPI]
public ref struct LimitedViewBuilder<T>
{
    public static implicit operator View<T>(LimitedViewBuilder<T> builder) => builder.GetView();

    private readonly T[] _array;
    private int _position;

    public ref T this[int index] => ref _array[index];
    public Span<T> this[Range range] => _array.AsSpan(range);

    internal LimitedViewBuilder(int capacity)
    {
        _array = new T[capacity];
        _position = 0;
    }

    public void Add(T item)
    {
        _array[_position++] = item;
    }

    public void AddMany(params ReadOnlySpan<T> items)
    {
        items.CopyTo(_array.AsSpan(_position));
    }

    public readonly View<T> GetView()
    {
        return new View<T>(_array);
    }
}