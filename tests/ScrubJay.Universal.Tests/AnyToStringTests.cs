using ScrubJay.Testing.Collections;
using ScrubJay.Testing.Validation;

namespace ScrubJay.Universal.Tests;

public partial class AnyToStringTests
{
    public static TheoryData<object?> ObjectData { get; } = new(TestingData.Objects);
    public static TheoryData<string?> StringData { get; } = new(TestingData.Strings.GetStringsAndNull(10));
    public static TheoryData<char> CharData { get; } = new(TestingData.Characters.ScaryCharacters);

    [Theory]
    [MemberData(nameof(ObjectData))]
    public void ObjectWorks(object? obj)
    {
        string? toString = obj?.ToString();
        string? anyToString = Any.ToString(obj);
        if (obj is null)
        {
            Assert.Null(toString);
            Assert.Null(anyToString);
        }
        else
        {
            Assert.Equal(toString, anyToString);
        }
    }

    [Theory]
    [MemberData(nameof(StringData))]
    public void StringWorks(string? str)
    {
        string? anyToString = Any.ToString<string>(in str);
        if (str is null)
        {
            Assert.Null(anyToString);
        }
        else
        {
            Assert.Equal(str, anyToString);
        }
    }
    
    [Theory]
    [MemberData(nameof(CharData))]
    public void CharWorks(char ch)
    {
        string? anyToString = Any.ToString<char>(in ch);
        Assert.NotNull(anyToString);
        Assert.Equal(1, anyToString.Length);
        Assert.Equal(ch, anyToString[0]);
    }
    
    
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void IntWorks(int i32)
    {
        string? toString = i32.ToString();
        string? anyToString = Any.ToString<int>(i32);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

 

   

}