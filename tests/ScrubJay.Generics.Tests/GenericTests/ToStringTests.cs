namespace ScrubJay.Generics.Tests.GenericTests;

public class ToStringTests
{
    [Fact]
    public void RefStructsWork()
    {
        string? toString;
        string? genericToString;

        ReadOnlySpan<char> text = "TRJ";
        toString = text.ToString();
        genericToString = Anythhing.ToString(in text);
        Assert.Equal(toString, genericToString);

        Span<byte> bytes = [1, 4, 7, 10, 13];
        toString = bytes.ToString();
        genericToString = Generic.ToString(in bytes);
        Assert.Equal(toString, genericToString);

        DefaultInterpolatedStringHandler dish = $"Hello, {text}";
        toString = dish.ToString();
        genericToString = Generic.ToString(in dish);
        Assert.Equal(toString, genericToString);

        FullTestRefStruct ftrf = new FullTestRefStruct(147, "TRJ");
        toString = ftrf.ToString();
        genericToString = Generic.ToString(in ftrf);
        Assert.Equal(toString, genericToString);
    }
}