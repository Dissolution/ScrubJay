namespace ScrubJay.Reflection.LightWeight.Tests;

public class AnyGetTypeTests
{
    [Fact]
    public void CannotRelyOnGenericType()
    {
        List<int> list = [1, 4, 7];
        var type = getType<IEnumerable>(list);
        Assert.Equal(typeof(IEnumerable), type);
        return;
        
        static Type getType<T>(ref readonly T? _)
        {
            return typeof(T);
        }
    }
    
    
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }
}