#pragma warning disable
// ReSharper disable All

using System.Globalization;
using System.Reflection;
using ScrubJay.Debugging;
using ScrubJay.Debugging.Destinations;
using ScrubJay.Errors;
using ScrubJay.Functional;
using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;
using ScrubJay.Sandboxes.ConsoleApp;
using ScrubJay.Universal;


var i = await Utils.ParseAsync("TRJ");

Debugger.Break();


Console.WriteLine("Press any key to exit.");
Console.ReadKey();
return;


namespace ScrubJay.Sandboxes.ConsoleApp
{
    public static class Utils
    {

        public static async Result<int> ParseAsync(string? str)
        {
            if (int.TryParse(str, NumberStyles.Any, null, out int i32))
                return i32;
            throw Ex.Parse<int>(str);
        }
        
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