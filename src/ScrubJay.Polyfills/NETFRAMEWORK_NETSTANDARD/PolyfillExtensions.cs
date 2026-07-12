#if NETFRAMEWORK || NETSTANDARD

using System.Runtime.CompilerServices;
using ScrubJay.Errors.Validation;


namespace ScrubJay.Polyfills;

public static partial class PolyfillExtensions
{
    extension(MemoryMarshal)
    {
        /// <summary>
        /// Returns a reference to the 0th element of <paramref name="array"/>. If the array is empty, returns a reference to where the 0th element
        /// would have been stored. Such a reference may be used for pinning but must never be dereferenced.
        /// </summary>
        /// <exception cref="NullReferenceException"><paramref name="array"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetArrayDataReference<T>(T[] array)
        {
            Throw.IfNull(array);
            return ref MemoryMarshal.GetReference(array.AsSpan());
        }
    }
}


#endif