using NaughtyStrings;
using ScrubJay.Testing;

namespace ScrubJay.Interpolated.Tests;

public class ImplicitTests
{
    public static TheoryData<string> TestStrings { get; } = new(TheNaughtyStrings.All);

    [Theory]
    [MemberData(nameof(TestStrings))]
    public void ImplicitFromStringWorks(string str)
    {
        InterpolatedTextHandler handler = str;

        var arrayRef = handler._rentedCharArray;

        var (hs, ha) = consume(handler);

        Demand.That(hs).IsEqualTo(str);
        Demand.That(ha).ReferenceEquals(arrayRef);
        
        handler.Dispose();
        Demand.That(ha).AllEqualTo('\0');

        static (string, char[]?) consume(InterpolatedTextHandler handler)
        {
            string handlerString = handler.ToString();
            var handlerArrayRef = handler._rentedCharArray;
            return (handlerString, handlerArrayRef);
        }
    }
}