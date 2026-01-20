// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace ScrubJay.Universal.Tests;

public class AnyToStringTests
{
    [Fact]
    public void DecimalsWork()
    {
        decimal m;
        Random rand = new Random();

        for (var i = 0; i < 100; i++)
        {
            m = (rand.Next(1000) + (rand.Next(1000) / 1000m));
            string toString = m.ToString();
            string anyToString = Any.ToString<decimal>(m);
            Assert.Equal(toString, anyToString);
        }
    }

    public static TheoryData<DateTime> DateTimes { get; } = new();

    static AnyToStringTests()
    {
        Random rand = new Random();

        while (DateTimes.Count < 100)
        {
            long ticks = rand.NextInt64();
            if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
                continue;
            DateTime datetime = new DateTime(ticks);
            DateTimes.Add(datetime);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimes))]
    public void DateTimesWork(DateTime dateTime)
    {
        string toString = dateTime.ToString();
        string anyToString = Any.ToString<DateTime>(dateTime);
        Assert.Equal(toString, anyToString);
    }

    [Fact]
    public void TestRefStructWorks()
    {
        TestRefStruct test = new TestRefStruct(147, "TRJ");
        string toString = test.ToString();
        string anyToString = Any.ToString<TestRefStruct>(test);
        Assert.Equal(toString, anyToString);
    }
}