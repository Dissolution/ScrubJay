namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class CharacterExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static text AsSpan(this scoped in char ch)
    {
#if NET7_0_OR_GREATERw
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