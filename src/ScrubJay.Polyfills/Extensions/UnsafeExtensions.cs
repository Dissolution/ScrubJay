namespace ScrubJay.Polyfills;

[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(Unsafe)
    {
        public static void SelfCopy<T>(T[] array, int sourceIndex, int destinationIndex, int count)
            where T : unmanaged
        {
            int byteCount = count * Unsafe.SizeOf<T>();
            fixed (void* sourcePtr = &Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), sourceIndex))
            fixed (void* destPtr = &Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), destinationIndex))
            {
                if (destinationIndex > sourceIndex && destinationIndex < sourceIndex + count)
                {
                    // cannot use cpblk
                    // ECMA 335: The behavior of cpblk is unspecified if the source and destination areas overlap
                    // MemoryCopy is overlap-safe.
                    Buffer.MemoryCopy(sourcePtr, destPtr, byteCount, byteCount);
                }
                else
                {
                    Unsafe.CopyBlockUnaligned(destPtr, sourcePtr, (uint)byteCount);
                }
            }
        }
    }

#if !NET10_0_OR_GREATER
    extension(Unsafe)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AsPointer<T>(ref readonly T value)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            Emit.Ldarg(nameof(value));
            Emit.Conv_U();
            return ReturnPointer();
        }
    }
#endif
}