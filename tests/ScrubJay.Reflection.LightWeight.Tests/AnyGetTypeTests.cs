namespace ScrubJay.Reflection.LightWeight.Tests;

public class AnyGetTypeTests
{
    [Fact]
    public void CannotRelyOnGenericType()
    {
        List<int> list = [1, 4, 7];
        var typeA = getTypeA<IEnumerable>(list);
        Assert.Equal(typeof(IEnumerable), typeA);

        var typeB = getTypeB<IEnumerable>(list);
        Assert.Equal(typeof(List<int>), typeB);

        return;

        static Type getTypeA<T>(ref readonly T? _)
        {
            return typeof(T);
        }

        static Type getTypeB<T>(ref readonly T? instance)
        {
            return instance?.GetType() ?? typeof(T);
        }
    }

    public static TheoryData<object?, Type?> TestObjectTypes { get; } =
    [
        (147, typeof(int)),
        (new List<byte>() { 1, 2, 147 }, typeof(List<byte>)),
        ('x', typeof(char)),
        (Guid.NewGuid(), typeof(Guid)),
        ((object)(IEnumerable)(IEnumerable<int>)(List<int>)[1, 4, 7], typeof(List<int>)),
        ((object)(IEnumerable<char>)"TRJ", typeof(string)),
        (null, typeof(object)),
    ];

    [Theory]
    [MemberData(nameof(TestObjectTypes))]
    public void ObjectInstanceGetTypeWorks(object? instance, Type? expectedType)
    {
        var getType = instance?.GetType();
        var anyGetType = Generic.GetType(instance);
        Assert.Equal(expectedType, anyGetType);
    }

    [Theory]
    [MemberData(nameof(TestObjectTypes))]
    public void GenericInstanceGetTypeWorks<I>(I? instance, Type? expectedType)
    {
        var getType = instance?.GetType();
        var anyGetType = Generic.GetType(instance);
        Assert.Equal(expectedType, anyGetType);
    }

    [Fact]
    public void GenericConstrainedInstanceGetTypeWorks()
    {
        var a = testGetType<int>(147);
        Assert.Equal(typeof(int), a);

        var b = testGetType<IEnumerable<char>>("TRJ");
        Assert.Equal(typeof(string), b);

        var c = testGetType<object>((object)(IEnumerable)(IEnumerable<int>)(List<int>)[1, 4, 7]);
        Assert.Equal(typeof(List<int>), c);

        text d = "T.R.Jay";
        var d_type = testGetType(d);
        Assert.Equal(typeof(text), d_type);

        return;

        static Type? testGetType<I>(in I? instance)
            where I : allows ref struct
        {
            return Generic.GetType<I>(in instance);
        }
    }
}