namespace ScrubJay.Reflection;

[PublicAPI]
public sealed class Attributes : IReadOnlyList<Attribute>
{
    public static Attributes For(MemberInfo member, bool inherit = true)
    {
        return new(Attribute.GetCustomAttributes(member, inherit));
    }
    
    public static Attributes For(ParameterInfo parameter, bool inherit = true)
    {
        return new(Attribute.GetCustomAttributes(parameter, inherit));
    }
    
    private readonly Attribute[] _attributes;

    public int Count => _attributes.Length;

    public Attribute this[int index] => _attributes[index];

    internal Attributes(Attribute[] attributes)
    {
        _attributes = attributes;
    }

    public bool Contains<A>()
        where A : Attribute
    {
        return _attributes.OfType<A>().Any();
    }

    public bool Contains<A>([NotNullWhen(true)] out A? attribute)
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