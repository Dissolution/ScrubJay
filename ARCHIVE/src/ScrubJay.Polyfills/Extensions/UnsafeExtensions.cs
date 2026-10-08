#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
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

        public static T* VoidPtrAsPtr<T>(void* pointer)
        {
            Emit.Ldarg(nameof(pointer));
            return ReturnPointer<T>();
        }
        
        public static ref T VoidPtrAsRef<T>(void* pointer)
        {
            Emit.Ldarg(nameof(pointer));
            return ref ReturnRef<T>();
        }
        
        
        public static void* InAsVoidPtr<T>(in T value)
        {
            Emit.Ldarg(nameof(value));
            return ReturnPointer();
        }
        
        public static T* InAsPtr<T>(in T value)
        {
            Emit.Ldarg(nameof(value));
            return ReturnPointer<T>();
        }
        
        public static ref T InAsRef<T>(in T value)
        {
            Emit.Ldarg(nameof(value));
            return ref ReturnRef<T>();
        }
        
        public static void* RefAsVoidPtr<T>(ref T value)
        {
            Emit.Ldarg(nameof(value));
            return ReturnPointer();
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