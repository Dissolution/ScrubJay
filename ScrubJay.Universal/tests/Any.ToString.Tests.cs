// ReSharper disable SpecifyACultureInStringConversionExplicitly
namespace ScrubJay.Universal.Tests;

public class AnyToStringTests
{

    [Fact]
    public void NumberWorks()
    {
        decimal m;
        Random rand =  new Random();
        
        for (var i = 0; i < 100; i++)
        {
            m = (rand.Next(1000) + (rand.Next(1000) / 1000m));
            string toString = m.ToString();
            string anyToString = Any.ToString<decimal>(m);
            
        }
    }
}