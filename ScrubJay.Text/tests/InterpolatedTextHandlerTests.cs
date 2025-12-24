using System.Runtime.CompilerServices;
using NaughtyStrings;
using ScrubJay.Testing;
using ScrubJay.Text;

namespace ScrubJay.Interpolated.Tests;

public class InterpolatedTextHandlerTests
{
    public static TheoryData<string> TestStrings { get; }
    public static TheoryData<char> TestCharacters { get; }

    static InterpolatedTextHandlerTests()
    {
        var naughtyStrings = TheNaughtyStrings.All;

        TestStrings = new(naughtyStrings);

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
    [MemberData(nameof(TestStrings))]
    public void DirectStringWorks(string str)
    {
        InterpolatedTextHandler handler = str;
        Demand.That(handler.ToString()).IsEqualTo(str);
    }

   


    /*

    [Fact]
    public void AlignTextWorks()
    {
        InterpolatedTextHandler handler;

        handler = $"|{SHORT,10}|";
        Demand.That(handler.ToString()).IsEqualTo("|    C0FFEE|");

        handler = $"|{SHORT,-10}|";
        Demand.That(handler.ToString()).IsEqualTo("|C0FFEE    |");

        handler = $"|{SHORT,4}|";
        Demand.That(handler.ToString()).IsEqualTo("|…FEE|");

        handler = $"|{SHORT,-4}|";
        Demand.That(handler.ToString()).IsEqualTo("|C0F…|");

        handler = $"|{SHORT,0}|";
        Demand.That(handler.ToString()).IsEqualTo("||");

        handler = $"|{SHORT,1}|";
        Demand.That(handler.ToString()).IsEqualTo("|…|");

        handler = $"|{SHORT,-1}|";
        Demand.That(handler.ToString()).IsEqualTo("|…|");

    }

    private struct Coffee
    {
        public override string ToString() => "C0FFEE";
    }

    [Fact]
    public void AlignValueWorks()
    {
        InterpolatedTextHandler handler;
        var coffee = new Coffee();

        // handler = $"|{coffee,10}|";
        // Demand.That(handler.ToString()).IsEqualTo("|    C0FFEE|");
        //
        // handler = $"|{coffee,-10}|";
        // Demand.That(handler.ToString()).IsEqualTo("|C0FFEE    |");
        //
        //handler = $"|{coffee,4}|";
        //Demand.That(handler.ToString()).IsEqualTo("|…FEE|");

        handler = $"|{coffee,-4}|";
        Demand.That(handler.ToString()).IsEqualTo("|C0F…|");

        handler = $"|{coffee,0}|";
        Demand.That(handler.ToString()).IsEqualTo("||");

        handler = $"|{coffee,1}|";
        Demand.That(handler.ToString()).IsEqualTo("|…|");

        handler = $"|{coffee,-1}|";
        Demand.That(handler.ToString()).IsEqualTo("|…|");

    }
    */
}