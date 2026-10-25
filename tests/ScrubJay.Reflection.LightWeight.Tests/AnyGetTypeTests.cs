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


    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }
}