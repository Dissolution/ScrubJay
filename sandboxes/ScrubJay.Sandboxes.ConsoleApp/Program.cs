#pragma warning disable
// ReSharper disable All

using ScrubJay.Polyfills.Universal;
text text = "TRJ";


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