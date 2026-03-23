// ReSharper disable EntityNameCapturedOnly.Global

using static InlineIL.IL;

namespace ScrubJay.Text.Utilities;

public static partial class TextHelper
{
    public static unsafe class Notsafe
    {
#region CopyBlock
        /* All the methods in here use the Cpblk instruction and have been specialized for use on char.
         *
         *`Cpblk(void* destination, void* source, nuint byteCount)`
         *
         * Source Types: void*, char*, ref readonly char, ReadOnlySpan<char>, Span<char>, char[], string
         * Destin Types: void*, char*, ref char, Span<char>, char[]
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
        public static void CopyBlock(void* source, void* destination, int count)
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
        public static void CopyBlock(void* source, char* destination, int count)
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
        public static void CopyBlock(void* source, ref char destination, int count)
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
        /// The destination <see cref="Span{char}">Span&lt;char&gt;</see> to copy characters to.
        /// </param>
        /// <param name="count">
        /// The total number of characters to copy.
        /// </param>
        /// <remarks>
        /// No validation nor bounds checks are performed in this method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(void* source, Span<char> destination, int count)
            => CopyBlock(source, ref MemoryMarshal.GetReference(destination), count);

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
        public static void CopyBlock(void* source, char[] destination, int count)
            => CopyBlock(source, ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(char* source, void* destination, int count)
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
        public static void CopyBlock(char* source, char* destination, int count)
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
        public static void CopyBlock(char* source, ref char destination, int count)
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
        public static void CopyBlock(char* source, Span<char> destination, int count)
            => CopyBlock(source, ref MemoryMarshal.GetReference(destination), count);

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
        public static void CopyBlock(char* source, char[] destination, int count)
            => CopyBlock(source, ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(ref readonly char source, void* destination, int count)
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
        public static void CopyBlock(ref readonly char source, char* destination, int count)
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
        public static void CopyBlock(ref readonly char source, ref char destination, int count)
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
        public static void CopyBlock(ref readonly char source, Span<char> destination, int count)
            => CopyBlock(in source, ref MemoryMarshal.GetReference(destination), count);

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
        public static void CopyBlock(ref readonly char source, char[] destination, int count)
            => CopyBlock(in source, ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(scoped ReadOnlySpan<char> source, void* destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), destination, count);

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
        public static void CopyBlock(scoped ReadOnlySpan<char> source, char* destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), destination, count);

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
        public static void CopyBlock(scoped ReadOnlySpan<char> source, ref char destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref destination, count);

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
        public static void CopyBlock(scoped ReadOnlySpan<char> source, Span<char> destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref MemoryMarshal.GetReference<char>(destination), count);

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
        public static void CopyBlock(scoped ReadOnlySpan<char> source, char[] destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(scoped Span<char> source, void* destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), destination, count);

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
        public static void CopyBlock(scoped Span<char> source, char* destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), destination, count);

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
        public static void CopyBlock(scoped Span<char> source, ref char destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref destination, count);

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
        public static void CopyBlock(scoped Span<char> source, Span<char> destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref MemoryMarshal.GetReference<char>(destination), count);

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
        public static void CopyBlock(scoped Span<char> source, char[] destination, int count)
            => CopyBlock(in MemoryMarshal.GetReference<char>(source), ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(char[] source, void* destination, int count)
            => CopyBlock(in MemoryMarshal.GetArrayDataReference<char>(source), destination, count);

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
        public static void CopyBlock(char[] source, char* destination, int count)
            => CopyBlock(in MemoryMarshal.GetArrayDataReference<char>(source), destination, count);

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
        public static void CopyBlock(char[] source, ref char destination, int count)
            => CopyBlock(in MemoryMarshal.GetArrayDataReference<char>(source), ref destination, count);

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
        public static void CopyBlock(char[] source, Span<char> destination, int count)
            => CopyBlock(in MemoryMarshal.GetArrayDataReference<char>(source), ref MemoryMarshal.GetReference<char>(destination), count);

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
        public static void CopyBlock(char[] source, char[] destination, int count)
            => CopyBlock(in MemoryMarshal.GetArrayDataReference<char>(source), ref MemoryMarshal.GetArrayDataReference(destination), count);
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
        public static void CopyBlock(string source, void* destination, int count)
            => CopyBlock(in source.GetPinnableReference(), destination, count);

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
        public static void CopyBlock(string source, char* destination, int count)
            => CopyBlock(in source.GetPinnableReference(), destination, count);

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
        public static void CopyBlock(string source, ref char destination, int count)
            => CopyBlock(in source.GetPinnableReference(), ref destination, count);

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
        public static void CopyBlock(string source, Span<char> destination, int count)
            => CopyBlock(in source.GetPinnableReference(), ref MemoryMarshal.GetReference<char>(destination), count);

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
        public static void CopyBlock(string source, char[] destination, int count)
            => CopyBlock(in source.GetPinnableReference(), ref MemoryMarshal.GetArrayDataReference(destination), count);
#endregion /Source: string




//        public static void SelfCopy(Span<char> chars, Range source, Range destination)
//        {
//            int length = chars.Length;
//            (int sourceOffset, int sourceLen) = source.UnsafeGetOffsetAndLength(length);
//            Debug.Assert(sourceOffset >= 0);
//            Debug.Assert(sourceLen >= 0);
//            Debug.Assert(sourceLen <= length);
//            (int destinationOffset, int destinationLength) = destination.UnsafeGetOffsetAndLength(length);
//            Debug.Assert(destinationOffset >= 0);
//            Debug.Assert(destinationLength >= 0);
//            Debug.Assert(destinationLength <= length);
//            Debug.Assert(destinationLength >= sourceLen);
//            ref char src = ref chars[sourceOffset];
//            ref char dst = ref chars[destinationOffset];
//            CopyBlock(in src, ref dst, sourceLen);
//        }
#endregion

#region Init
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitCharBlock(ref char source, int count)
        {
            /*         Instruction      ||  Stack */
            Emit.Ldarg(nameof(source)); //  char*
            Emit.Ldc_I4_0(); //             char* | 0_i32
            Emit.Ldarg(nameof(count)); //   char* | 0_i32 | count_i32
            Emit.Ldc_I4_1(); //             char* | 0_i32 | count_i32 | 1_i32
            Emit.Shl(); //                  char* | 0_i32 | (count*2)_i32
            Emit.Initblk(); //              _
        }
#endregion
//
//        // REALLY BAD
//        public static Span<char> AsWritableSpan(text text)
//        {
//            return new Span<char>(Utilities.Notsafe.InAsVoidPtr(in text.GetPinnableReference()), text.Length);
//        }
//
//        public static Span<char> AsWritableSpan(string? str)
//        {
//            if (str!.IsEmpty)
//                return default;
//
//            return new Span<char>(Utilities.Notsafe.InAsVoidPtr(in str.GetPinnableReference()), str.Length);
//        }
    }

#region Init
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(scoped Span<char> chars)
        => Notsafe.InitCharBlock(ref MemoryMarshal.GetReference(chars), chars.Length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(char[]? chars)
    {
        if (chars is not null)
        {
#if NET5_0_OR_GREATER
            Notsafe.InitCharBlock(ref MemoryMarshal.GetArrayDataReference(chars), chars.Length);
#else
            Notsafe.InitCharBlock(ref chars[0], chars.Length);
#endif
        }
    }
#endregion
}