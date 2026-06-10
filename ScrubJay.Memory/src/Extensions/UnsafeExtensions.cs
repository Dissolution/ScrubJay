namespace ScrubJay.Memory.Extensions;

/// <summary>
/// Extensions <b>on</b> <see cref="System.Runtime.CompilerServices.Unsafe"/> (that are also <see langword="unsafe"/>).
/// </summary>
[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(Unsafe)
    {
        public static void* InAsVoidPtr<T>(in T value)
        {
            Emit.Ldarg(nameof(value));
            return ReturnPointer();
        }
        
        
        public static void* RefAsVoidPtr<T>(ref T value)
        {
            Emit.Ldarg(nameof(value));
            return ReturnPointer();
        }
    }
}