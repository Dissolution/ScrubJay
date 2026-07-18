#pragma warning disable
// ReSharper disable All

using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Debugging;
using ScrubJay.Debugging.Destinations;
using ScrubJay.Errors;
using ScrubJay.Functional;
using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection;
using ScrubJay.Reflection.Extensions;
using ScrubJay.Sandboxes.ConsoleApp;
using ScrubJay.Universal;

var opcodes = OpCodes.All;


Debugger.Break();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();
return;


namespace ScrubJay.Sandboxes.ConsoleApp
{
    public static class Utils
    {
        public static async ValueTask<Guid> GetGuidAsync() => Guid.NewGuid();

        public static async Result<double> ParseAsync(string? str)
        {
            if (double.TryParse(str, out var f64))
                return f64;
            var error = Ex.Parse<double>(str);
            throw error;
        }

        public static async Result<double> DivideAsync(string? num, string? denom)
        {
            Console.Write("");
            _ = await GetGuidAsync();
            Console.Write("");
            await Task.Delay(500);
            _ = await GetGuidAsync();
            Console.Write("");
            double n = await ParseAsync(num);
            _ = await GetGuidAsync();
            await Task.Delay(300);
            _ = await GetGuidAsync();
            Console.Write("");
            double d = await ParseAsync(denom);
            _ = await GetGuidAsync();
            await Task.Delay(200);
            _ = await GetGuidAsync();
            Console.Write("");
            if (d == 0d)
                throw new DivideByZeroException();
            _ = await GetGuidAsync();
            await Task.Delay(100);
            _ = await GetGuidAsync();
            Console.Write("");
            return n / d;
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