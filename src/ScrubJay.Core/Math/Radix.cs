using ScrubJay.Errors;
using ScrubJay.Text;

namespace ScrubJay.Math;

[PublicAPI]
public static partial class Radix
{
    public static Result<string, Exception> TryEncode(
        ulong number,
        scoped text symbols)
    {
        var radix = (ulong)symbols.Length;
        if (radix < 2UL)
            return Ex.ArgRange(symbols, "There must be at least 2 symbols.");

        using Interpolator buffer = stackalloc char[1024];
        do
        {
            var symbolIndex = number % radix;
            Debug.Assert(symbolIndex < radix);
            buffer.Write(symbols[(int)symbolIndex]);
            number /= radix;
        } while (number > 0UL);

        // have to reverse the symbols
        buffer.Written.Reverse();
        return buffer.ToString();
    }

    public static Result<string, Exception> TryEncode(
        BigInteger number,
        scoped text symbols)
    {
        if (number <= BigInteger.Zero)
            return Ex.ArgRange(number, "must be Zero or greater.");
        BigInteger radix = symbols.Length;
        if (radix <= BigInteger.One)
            return Ex.ArgRange(symbols, "must be at least 2 symbols.");

        using Interpolator buffer = stackalloc char[1024];
        do
        {
            var symbolIndex = number % radix;
            Debug.Assert(symbolIndex < radix);
            buffer.Write(symbols[(int)symbolIndex]);
            number /= radix;
        } while (number > BigInteger.Zero);

        // have to reverse the symbols
        buffer.Written.Reverse();
        return buffer.ToString();
    }

    public static Result<string, Exception> TryEncode<N>(
        N number,
        scoped text symbols)
        where N : INumber<N>
    {
        if (number <= N.Zero)
            return Ex.ArgRange(number, "must be Zero or greater.");
        N radix = N.CreateChecked(symbols.Length);
        if (radix <= N.One)
            return Ex.ArgRange(symbols, "must be at least 2 symbols.");

        using Interpolator buffer = stackalloc char[1024];
        do
        {
            var symbolIndex = number % radix;
            Debug.Assert(symbolIndex < radix);
            buffer.Write(symbols[int.CreateChecked(symbolIndex)]);
            number /= radix;
        } while (number > N.Zero);

        // have to reverse the symbols
        buffer.Written.Reverse();
        return buffer.ToString();
    }

    public static Result<N, Exception> TryDecode<N>(
        scoped text radixed,
        scoped text symbols)
        where N : INumber<N>
    {
        if (radixed.IsEmpty)
            return Ex.ArgRange(radixed, "encoded string was invalid");
        var radix = symbols.Length;
        if (radix <= 1)
            return Ex.ArgRange(symbols, "must be at least 2 symbols.");

        N number = N.Zero;

        int i = 0;
        int len = radixed.Length;
        while (i < len)
        {
            char ch = radixed[i];
            int symbolIndex = symbols.IndexOf(ch);
            if (symbolIndex < 0)
                return Ex.Arg(radixed, $"character '{ch}' was not present in symbols \"{symbols}\"");

            i++;
            number += N.CreateChecked(symbolIndex * BigInteger.Pow(radix, len - i));
        }

        return number;
    }
}

partial class Radix
{
    public static class Symbols
    {
        /// <summary>
        /// Binary
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Binary_number"/>
        public const string BASE2 = "01";

        /// <summary>
        /// Senary
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Senary"/>
        public const string BASE6 = "012345";

        /// <summary>
        /// Octal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Octal"/>
        public const string BASE8 = "01234567";
        public const string BASE8_BAGUA = "☷☳☵☱☶☲☴☰";

        /// <summary>
        /// Decimal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Decimal"/>
        public const string BASE10 = "0123456789";

        /// <summary>
        /// Duodecimal / Dozenal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Duodecimal"/>
        public const string BASE12 = "0123456789AB";
        public const string BASE12_PITMAN = "0123456789↊↋";

        /// <summary>
        /// Hexadecimal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Hexadecimal"/>
        public const string BASE16 = "0123456789ABCDEF";

        /// <summary>
        /// Vigesimal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Vigesimal"/>
        public const string BASE20 = "0123456789ABCDEFGHIJ";

        /// <summary>
        /// Base32
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Base32"/>
        public const string BASE32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        public const string BASE32_EXT_HEX = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        public const string BASE32_WORDSAFE = "23456789CFGHJMPQRVWXcfghjmpqrvwx";

        /// <summary>
        /// Hexatrigesimal
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Base36"/>
        public const string BASE36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Base62
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Base62"/>
        public const string BASE62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Base64
        /// </summary>
        /// <seealso cref="https://en.wikipedia.org/wiki/Base64"/>
        public const string BASE64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public const string BASE64_URL = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
    }
}