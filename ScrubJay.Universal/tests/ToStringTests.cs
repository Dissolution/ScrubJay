using ScrubJay.Universal.Tests.Internal;

namespace ScrubJay.Universal.Tests;

public partial class ToStringTests
{
    public static TheoryData<object?> ObjectData { get; } = new(TestTypes.Objects);

    [Theory]
    [MemberData(nameof(ObjectData))]
    public void ObjectToStringWorks(object? obj)
    {
        string? toString = obj?.ToString();
        string? anyToString = Any.ToString(obj);
        
        Assert.Equal(toString, anyToString);
    }
}