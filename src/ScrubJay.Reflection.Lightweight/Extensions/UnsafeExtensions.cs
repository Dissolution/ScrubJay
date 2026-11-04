
using static InlineIL.IL;

namespace ScrubJay.Reflection.Lightweight.Extensions;

[PublicAPI]
public static class UnsafeExtensions
{
    extension(Unsafe)
    {
        public static nuint GetAddress<T>(ref readonly T value)
            where T : allows ref struct
        {
            Emit.Ldarg(nameof(value));
            Emit.Conv_U();
            return Return<nuint>();
        }
    }
}