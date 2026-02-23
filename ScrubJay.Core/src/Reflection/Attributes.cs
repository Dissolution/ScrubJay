namespace ScrubJay.Reflection;

public sealed class Attributes : IReadOnlyList<Attribute>
{
    private Attribute[] _attributes;

    public int Count => _attributes.Length;

    public Attribute this[int index]
    {
        get
        {
            return _attributes[Guard.Index(index, Count)];
        }
    }

    internal Attributes(Attribute[] attributes)
    {
        _attributes = attributes;
    }

    public bool HasAttribute<A>()
        where A : Attribute
    {
        return _attributes.OfType<A>().Any();
    }

    public bool HasAttribute<A>([NotNullWhen(true)] out A? attribute)
        where A : Attribute
    {
        attribute = _attributes.OfType<A>().FirstOrDefault();
        return attribute is not null;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Attribute> GetEnumerator()
    {
        foreach (Attribute attribute in _attributes)
        {
            yield return attribute;
        }
    }
}