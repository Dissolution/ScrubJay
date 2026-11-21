namespace ScrubJay.Collections;

[PublicAPI]
public struct ViewBuilder<T>
{
    public static implicit operator View<T>(ViewBuilder<T> builder) => builder.GetView();

    private List<T>? _list;

    public ref T this[int index] => ref CollectionsMarshal.AsSpan(_list)[index];

    public int Count => _list?.Count ?? 0;

    public ViewBuilder()
    {
    }

    public ViewBuilder(int capacity)
    {
        _list = new List<T>(capacity: capacity);
    }

    public void Add(T item)
    {
        _list ??= new();
        _list.Add(item);
    }

    [OverloadResolutionPriority(100)]
    public void AddMany(params ReadOnlySpan<T> items)
    {
        _list ??= new();
        _list.AddRange(items);
    }

    public void AddMany(IEnumerable<T>? items)
    {
        if (items is not null)
        {
            _list ??= new();
            _list.AddRange(items);
        }
    }

    public void InsertMany(int index, params ReadOnlySpan<T> items)
    {
        _list ??= new();
        _list.InsertRange(index, items);
    }

    public readonly View<T> GetView()
    {
        return View.Create<T>(_list);
    }
}