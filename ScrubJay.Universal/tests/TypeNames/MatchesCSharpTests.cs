using ScrubJay.Functional;
using ScrubJay.Validation;

namespace ScrubJay.Interpolated.Tests.TypeNames;


public class MatchesCSharpTests
{
    public static TypeExpectedData TestData { get; }

    static MatchesCSharpTests()
    {
        TestData = new(AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectWhere(static assembly => Try(assembly.GetTypes))
            .SelectMany(static types => types)
            .Select(static type => new TypeExpected(type, TypeToStringConverter.ToCSharpString(type))));
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void Works(TypeExpected te)
    {
        var (type, expected) = te;
        var name = TypeName.For(type);
        Demand.That(name).IsEqualTo(expected);
    }
}