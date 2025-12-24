using NaughtyStrings;
using ScrubJay.Testing;
using ScrubJay.Text;

namespace ScrubJay.Interpolated.Tests;

public class AppendStringTests
{
    public static TheoryData<string> TestStrings { get; } = new(TheNaughtyStrings.All);

    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AppendLiteralWorks(string str)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendLiteral(str);

        Demand.That(handler.Length).IsEqualTo(str.Length);
        Demand.That(handler.Written).IsEqualTo(str);

        const string SPHINX = "Sphinx of black quartz, judge my vow!";
        const string FOX = "The quick brown fox jumped over the lazy dog.";

        handler = $"Sphinx of black quartz, judge my vow!";
        Demand.That(handler.Written).IsEqualTo(SPHINX);

        handler = $"The quick brown fox jumped over the lazy dog.";
        Demand.That(handler.Written).IsEqualTo(FOX);
    }

    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AppendFormattedWorks(string str)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(str);

        Demand.That(handler.Length).IsEqualTo(str.Length);
        Demand.That(handler.Written).IsEqualTo(str);

        handler = $"{str}";

        Demand.That(handler.Length).IsEqualTo(str.Length);
        Demand.That(handler.Written).IsEqualTo(str);
    }

    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AlignZeroWorks(string str)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(str, 0);

        Demand.That(handler.Written).IsEmpty();

        handler = $"{str,0}";

        Demand.That(handler.Written).IsEmpty();
    }

    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AlignOneWorks(string str)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(str, 1);

        Demand.That(handler.Written.Length).IsEqualTo(1);
        
        if (str.Length == 0)
        {
            Demand.That(handler.Written[0]).IsEqualTo(' ');
        }
        else if (str.Length == 1)
        {
            Demand.That(handler.Written[0]).IsEqualTo(str[0]);
        }
        else
        {
            Demand.That(handler.Written[0]).IsEqualTo(InterpolatedTextHandler.ELLIPSIS);
        }

        handler = $"{str,1}";
        
        Demand.That(handler.Written.Length).IsEqualTo(1);
        
        if (str.Length == 0)
        {
            Demand.That(handler.Written[0]).IsEqualTo(' ');
        }
        else if (str.Length == 1)
        {
            Demand.That(handler.Written[0]).IsEqualTo(str[0]);
        }
        else
        {
            Demand.That(handler.Written[0]).IsEqualTo(InterpolatedTextHandler.ELLIPSIS);
        }
    }
    
    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AlignNegativeOneWorks(string str)
    {
        InterpolatedTextHandler handler = new();

        handler.AppendFormatted(str, -1);

        Demand.That(handler.Written.Length).IsEqualTo(1);
        
        if (str.Length == 0)
        {
            Demand.That(handler.Written[0]).IsEqualTo(' ');
        }
        else if (str.Length == 1)
        {
            Demand.That(handler.Written[0]).IsEqualTo(str[0]);
        }
        else
        {
            Demand.That(handler.Written[0]).IsEqualTo(InterpolatedTextHandler.ELLIPSIS);
        }

        handler = $"{str,-1}";
        
        Demand.That(handler.Written.Length).IsEqualTo(1);
        
        if (str.Length == 0)
        {
            Demand.That(handler.Written[0]).IsEqualTo(' ');
        }
        else if (str.Length == 1)
        {
            Demand.That(handler.Written[0]).IsEqualTo(str[0]);
        }
        else
        {
            Demand.That(handler.Written[0]).IsEqualTo(InterpolatedTextHandler.ELLIPSIS);
        }
    }

    /*
    [Theory]
    [MemberData(nameof(TestStrings))]
    public void AppendFormattedTextAlignRightExtraWorks(char ch)
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
    [MemberData(nameof(TestTextacters))]
    public void AppendFormattedTextAlignLeftExtraWorks(char ch)
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
    */
}