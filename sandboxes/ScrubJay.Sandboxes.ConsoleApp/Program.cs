#pragma warning disable
// ReSharper disable All

using ScrubJay.Polyfills;
using ScrubJay.Universal;
text text = "TRJ";

HashSet<int> numbers = [1, 4, 7];
numbers.ForEach(i =>  Console.WriteLine(i));

Debugger.Break();

Console.WriteLine("Press any key to exit.");
Console.ReadKey();
return;


namespace ScrubJay.Sandboxes.ConsoleApp
{
    public static class Utils
    {

#if NET9_0_OR_GREATER
        [return: NotNullIfNotNull(nameof(value))]
        public static string? AnyToString<T>(in T? value)
            where T : allows ref struct
        {
            return Any.ToString(in value);
        }
#endif
    }
}