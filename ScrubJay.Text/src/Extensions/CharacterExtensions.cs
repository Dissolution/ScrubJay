namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class CharacterExtensions
{
    extension(ref readonly char ch)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public text AsSpan()
        {
#if NET7_0_OR_GREATER
            return new text(in ch);
#else
            IL.Emit.Ldarg(nameof(ch));
            IL.Emit.Ldc_I4_1();
            IL.Emit.Call(MethodRef.Constructor(typeof(text), [typeof(void*), typeof(int)]));
            IL.Emit.Ret();
            throw Unreachable();
#endif
        }
    }
}