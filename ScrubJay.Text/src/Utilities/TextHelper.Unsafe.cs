// ReSharper disable EntityNameCapturedOnly.Global

namespace ScrubJay.Text.Utilities;

public static partial class TextHelper
{
    /// <summary>
    /// <see langword="unsafe"/> methods working with textual types.
    /// </summary>
    public static unsafe class Unsafe
    {
#region CopyTo
        /* Specialized CopyBlock version of text copying for maximum speed
         * No null checks, no length checks
         *   Source Types: void*, char*, ref readonly char, ReadOnlySpan<char>, char[], string
         *   Destin Types: void*, char*, ref char, Span<char>, char[]
         */


#region Source: void*
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="void*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(void* source, void* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="void*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(void* source, char* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="void*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(void* source, ref char destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="void*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{T}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(void* source, Span<char> destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(source, dest, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="void*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(void* source, char[] destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(source, dest, count);
            }
        }
#endregion /Source: void*

#region Source: char*
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="char*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, void* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="char*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, char* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="char*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, ref char destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="char*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, Span<char> destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(source, dest, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="char*"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, char[] destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(source, dest, count);
            }
        }
#endregion /Source: char*

#region Source: ref readonly char
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="ref readonly"/> <see cref="char"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, void* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="ref readonly"/> <see cref="char"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, char* destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="ref readonly"/> <see cref="char"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, ref char destination, int count)
        {
            Emit.Ldarg(nameof(destination));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="ref readonly"/> <see cref="char"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, Span<char> destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(in source, dest, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see langword="ref readonly"/> <see cref="char"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, char[] destination, int count)
        {
            fixed (char* dest = destination)
            {
                CopyTo(in source, dest, count);
            }
        }
#endregion /Source: ref readonly char

#region Source: ReadOnlySpan<char>
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="ReadOnlySpan{char}">ReadOnlySpan&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped ReadOnlySpan<char> source, void* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="ReadOnlySpan{char}">ReadOnlySpan&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped ReadOnlySpan<char> source, char* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="ReadOnlySpan{char}">ReadOnlySpan&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped ReadOnlySpan<char> source, ref char destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, ref destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="ReadOnlySpan{char}">ReadOnlySpan&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped ReadOnlySpan<char> source, Span<char> destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="ReadOnlySpan{char}">ReadOnlySpan&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped ReadOnlySpan<char> source, char[] destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }
#endregion /Source: ReadOnlySpan<char>

#region Source: Span<char>
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped Span<char> source, void* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped Span<char> source, char* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped Span<char> source, ref char destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, ref destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped Span<char> source, Span<char> destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(scoped Span<char> source, char[] destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }
#endregion /Source: Span<char>

#region Source: char[]
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="char"/><see langword="[]"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char[] source, void* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="char"/><see langword="[]"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char[] source, char* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="char"/><see langword="[]"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char[] source, ref char destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, ref destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="char"/><see langword="[]"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char[] source, Span<char> destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="char"/><see langword="[]"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char[] source, char[] destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }
#endregion /Source: char[]

#region Source: string
        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="void*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(string source, void* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char*"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(string source, char* destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="ref char"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(string source, ref char destination, int count)
        {
            fixed (char* src = source)
            {
                CopyTo(src, ref destination, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(string source, Span<char> destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }

        /// <summary>
        /// Copy <paramref name="count"/> <see cref="char">characters</see>
        /// from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/> to copy characters from.
        /// </param>
        /// <param name="destination">
        /// The destination <see langword="char[]"/> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(string source, char[] destination, int count)
        {
            fixed (char* src = source)
            fixed (char* dst = destination)
            {
                CopyTo(src, dst, count);
            }
        }
#endregion /Source: string
#endregion

#region Init
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitCharBlock(char* source, int count)
        {
            Emit.Ldarg(nameof(source));
            Emit.Ldc_I4_0();
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Initblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitCharBlock(ref char source, int count)
        {
            Emit.Ldarg(nameof(source));
            Emit.Ldc_I4_0();
            Emit.Ldarg(nameof(count));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Initblk();
        }
#endregion

#region SelfCopy
        public static void SelfCopy(char[] array, int sourceIndex, int destIndex, int count)
        {
#if DEBUG
            Debug.Assert(array is not null);
            int arrayLength = array!.Length;

            Debug.Assert(arrayLength > 0);
            Debug.Assert(count > 0);

            Debug.Assert(sourceIndex >= 0 && sourceIndex <= arrayLength);
            int sourceEnd = sourceIndex + count;
            Debug.Assert(sourceEnd >= 0 && sourceEnd <= arrayLength);

            Debug.Assert(destIndex >= 0 && destIndex <= arrayLength);
            int destEnd = destIndex + count;
            Debug.Assert(destEnd >= 0 && destEnd <= arrayLength);
#endif

            ref char src = ref array[sourceIndex];
            ref char dst = ref array[destIndex];
            CopyTo(ref src, ref dst, count);
        }

        public static void SelfCopy(char[] chars, Range source, int destStart)
        {
            Debug.Assert(chars is not null);
            int len = chars!.Length;
            Debug.Assert(len > 0);

            int sourceStart = source.Start.GetOffset(len);
            Debug.Assert(sourceStart >= 0 && sourceStart <= len);

            int sourceEnd = source.End.GetOffset(len);
            Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

            int sourceLength = sourceEnd - sourceStart;
            Debug.Assert(sourceLength >= 0 && sourceLength <= len);

            Debug.Assert(destStart >= 0 && destStart <= len);

            int destEnd = destStart + sourceLength;
            Debug.Assert(destEnd >= 0 && destEnd <= len);

            int endLength = destEnd - destStart;
            Debug.Assert(endLength >= 0 && endLength <= len);

            Debug.Assert(endLength >= sourceLength);

            ref char src = ref chars[sourceStart];
            ref char dst = ref chars[destStart];
            CopyTo(ref src, ref dst, sourceLength);
        }

        public static void SelfCopy(char[] chars, Range source, Index dest)
        {
            Debug.Assert(chars is not null);
            int len = chars!.Length;
            Debug.Assert(len > 0);

            int sourceStart = source.Start.GetOffset(len);
            Debug.Assert(sourceStart >= 0 && sourceStart <= len);

            int sourceEnd = source.End.GetOffset(len);
            Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

            int sourceLength = sourceEnd - sourceStart;
            Debug.Assert(sourceLength >= 0 && sourceLength <= len);

            int destStart = dest.GetOffset(len);
            Debug.Assert(destStart >= 0 && destStart <= len);

            int destEnd = destStart + sourceLength;
            Debug.Assert(destEnd >= 0 && destEnd <= len);

            int endLength = destEnd - destStart;
            Debug.Assert(endLength >= 0 && endLength <= len);

            Debug.Assert(endLength >= sourceLength);

            ref char src = ref chars[sourceStart];
            ref char dst = ref chars[destStart];
            CopyTo(ref src, ref dst, sourceLength);
        }

        public static void SelfCopy(char[] chars, Range source, Range dest)
        {
            Debug.Assert(chars is not null);
            int len = chars!.Length;
            Debug.Assert(len > 0);

            int sourceStart = source.Start.GetOffset(len);
            Debug.Assert(sourceStart >= 0 && sourceStart <= len);

            int sourceEnd = source.End.GetOffset(len);
            Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

            int sourceLength = sourceEnd - sourceStart;
            Debug.Assert(sourceLength >= 0 && sourceLength <= len);

            int destStart = dest.Start.GetOffset(len);
            Debug.Assert(destStart >= 0 && destStart <= len);

            int destEnd = dest.End.GetOffset(len);
            Debug.Assert(destEnd >= 0 && destEnd <= len);

            int endLength = destEnd - destStart;
            Debug.Assert(endLength >= 0 && endLength <= len);

            Debug.Assert(endLength >= sourceLength);

            ref char src = ref chars[sourceStart];
            ref char dst = ref chars[destStart];
            CopyTo(ref src, ref dst, sourceLength);
        }
#endregion


#region Shift
        internal static void ShiftItemsRight(
            char[] array, int arrayUsed,
            int index, int length)
        {
#if DEBUG
            Debug.Assert(array is not null);
            int arrayLength = array!.Length;
            Debug.Assert(arrayLength > 0);
            Debug.Assert(arrayUsed >= 0 && arrayUsed <= arrayLength);
            Debug.Assert(index >= 0 && index < arrayUsed);
            Debug.Assert(length > 0 && length <= arrayLength);
            Debug.Assert(arrayUsed + length <= arrayLength);
#endif

            CopyTo(ref array[index], ref array[index + length], arrayUsed - index);
        }

        internal static void ShiftItemsLeft(
            char[] array, int arrayUsed,
            int index, int length)
        {
#if DEBUG
            Debug.Assert(array is not null);
            int arrayLength = array!.Length;
            Debug.Assert(arrayLength > 0);
            Debug.Assert(arrayUsed >= 0 && arrayUsed <= arrayLength);
            Debug.Assert(index >= 0 && index < arrayUsed);
            Debug.Assert(length > 0 && length <= arrayUsed);
#endif

            CopyTo(ref array[index + length], ref array[index], arrayUsed - (index + length));
        }
#endregion
    }

}