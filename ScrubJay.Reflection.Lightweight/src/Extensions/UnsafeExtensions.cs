using InlineIL;

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class UnsafeExtensions
{
    extension(Unsafe)
    {
        /// <summary>
        /// When you have nothing else you can know about a value, you can at least get the bytes of the reference
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> GetReferenceBytes<T>(scoped ref readonly T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        {
            Emit.Ldarg(nameof(value));
            Emit.Conv_U();
            Emit.Sizeof<nuint>();
            Emit.Newobj(MethodRef.Constructor(typeof(ReadOnlySpan<byte>), [typeof(void*), typeof(int)]));
            Emit.Ret();
            throw Unreachable();
        }
    }
}