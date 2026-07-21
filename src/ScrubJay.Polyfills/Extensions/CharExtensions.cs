using InlineIL;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class CharExtensions
{
    extension(ref readonly char ch)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public text AsSpan()
        {
#if NET7_0_OR_GREATER
            return new text(in ch);
#else
            Emit.Ldarg(nameof(ch));
            Emit.Ldc_I4_1();
            Emit.Call(MethodRef.Constructor(typeof(text), [typeof(void*), typeof(int)]));
            Emit.Ret();
            throw Unreachable();
#endif
        }
    }
}