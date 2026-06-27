#if !NET7_0_OR_GREATER

namespace ScrubJay.Polyfills;

public static partial class PolyfillExtensions
{
    extension(int)
    {
        public static bool IsEvenInteger(int value) => (value & 1) == 0;
    }
}


#endif