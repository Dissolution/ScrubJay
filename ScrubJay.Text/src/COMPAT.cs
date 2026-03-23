#pragma warning disable all
using static InlineIL.IL;

#if !NET6_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    /// <summary>Indicates the attributed type is to be used as an interpolated string handler.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class InterpolatedStringHandlerAttribute : Attribute
    {
        /// <summary>Initializes the <see cref="InterpolatedStringHandlerAttribute"/>.</summary>
        public InterpolatedStringHandlerAttribute() { }
    }
}

internal static class CompatExtensions
{
    extension(MemoryMarshal)
    {
        public static ref T GetArrayDataReference<T>(T[] array)
        {
            Emit.Ldarg_0();
            Emit.Ldc_I4_0();
            Emit.Ldelema<T>();
            return ref ReturnRef<T>();
        }
    }

    extension(string str)
    {
        public ref char GetPinnableReference()
        {
            unsafe
            {
                fixed (char* ptr = str)
                {
                    return ref Unsafe.AsRef<char>(ptr);
                }
            }
        }
    }

#if NETSTANDARD2_1
    private static unsafe class Unsafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(T* ptr)
        {
            Emit.Ldarg(nameof(ptr));
            return ref ReturnRef<T>();
        }
    }
#endif
}
#endif