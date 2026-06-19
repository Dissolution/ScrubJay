
using ScrubJay.Functional;
using static ScrubJay.Functional.Prelude;



Result<int, Exception> result = new(147);

var k = result switch
{
    int i => $"Ok: {i}",
    Exception ex => $"Error: {ex}",
};

var eq = result.Equals(13);

Console.WriteLine(k);
Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
return;