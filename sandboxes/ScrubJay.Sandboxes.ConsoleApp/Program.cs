#pragma warning disable
// ReSharper disable All

using System.Reflection;
using ScrubJay.Debugging.Destinations;
using ScrubJay.Functional;
using ScrubJay.Polyfills;
using ScrubJay.Universal;


Result<int, Exception> result = new(147);
Result<object, Exception> other = new(147);

var fmt = result.ToString("D");

ConsoleColors cc = default;



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