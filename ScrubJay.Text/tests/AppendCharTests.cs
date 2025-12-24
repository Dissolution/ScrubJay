using NaughtyStrings;
using ScrubJay.Testing;
using ScrubJay.Text;

namespace ScrubJay.Interpolated.Tests;

public class AppendCharTests
{
    public static TheoryData<char> TestCharacters { get; }

    static AppendCharTests()
    {
        var naughtyStrings = TheNaughtyStrings.All;

        HashSet<char> naughtyChars = new();
        for (var i = 0; i < naughtyStrings.Count; i++)
        {
            ReadOnlySpan<char> naughty = naughtyStrings[i];
            foreach (char ch in naughty)
            {
                naughtyChars.Add(ch);
            }
        }

        TestCharacters = new(naughtyChars);
    }

    [Theory]
    [MemberData(nameof(TestCharacters))]
    public void AppendFormattedWorks(char ch)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(ch);

        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);

        handler = $"{ch}";

        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);
    }

    [Theory]
    [MemberData(nameof(TestCharacters))]
    public void AlignZeroWorks(char ch)
    {
        InterpolatedTextHandler handler = new();

        
        handler.AppendFormatted(ch, 0);

        Demand.That(handler.Length).IsEqualTo(0);

        handler = $"{ch,0}";

        Demand.That(handler.Length).IsEqualTo(0);
    }

    [Theory]
    [MemberData(nameof(TestCharacters))]
    public void AlignOneWorks(char ch)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(ch, 1);

        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);

        handler = new();
        handler.AppendFormatted(ch, -1);
        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);

        handler = $"{ch,1}";
        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);

        handler = $"{ch,-1}";
        Demand.That(handler.Length).IsEqualTo(1);
        Demand.That(handler.Written[0]).IsEqualTo(ch);
    }

    [Theory]
    [MemberData(nameof(TestCharacters))]
    public void AlignRightExtraWorks(char ch)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(ch, 5);

        Demand.That(handler.Length).IsEqualTo(5);
        Demand.That(handler.Written[..4]).AllEqualTo(' ');
        Demand.That(handler.Written[4]).IsEqualTo(ch);

        handler = $"{ch,5}";
        Demand.That(handler.Length).IsEqualTo(5);
        Demand.That(handler.Written[..4]).AllEqualTo(' ');
        Demand.That(handler.Written[4]).IsEqualTo(ch);
    }

    [Theory]
    [MemberData(nameof(TestCharacters))]
    public void AlrightLeftExtraWorks(char ch)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(ch, -5);

        Demand.That(handler.Length).IsEqualTo(5);
        Demand.That(handler.Written[0]).IsEqualTo(ch);
        Demand.That(handler.Written[1..5]).AllEqualTo(' ');

        handler = $"{ch,-5}";
        Demand.That(handler.Length).IsEqualTo(5);
        Demand.That(handler.Written[0]).IsEqualTo(ch);
        Demand.That(handler.Written[1..5]).AllEqualTo(' ');
    }
}