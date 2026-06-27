using ScrubJay.Polyfills.Universal;
text text = "TRJ";

//var str = Utils.AnyToString(text);
//var str2 = Utils.AnyToString(147);
//


Console.WriteLine("Press any key to exit.");
Console.ReadKey();
return;


namespace ScrubJay.Sandboxes.Consoleapp
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