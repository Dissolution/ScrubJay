namespace ScrubJay.Universal.Tests.TypeNames;


public class MatchesCSharpTests
{
    public static TypeExpectedData TestData { get; }

    static MatchesCSharpTests()
    {
        TestData = new(AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(static assembly => assembly.GetTypes())
            .Select(static type => new TypeExpected(type, TypeToStringConverter.ToCSharpString(type))));
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void Works(TypeExpected te)
    {
        var (type, expected) = te;
        var name = TypeName.For(type);
        Assert.Equal(expected, name);
    }
}