#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace ScrubJay.Universal.UNPROCESSED;

partial class Any
{
    /// <summary>
    /// Gets a hashcode for a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to hash, may be <c>null</c>.
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? value)
    {
        if (value is null)
            return 0;

        return value.GetHashCode();
    }

    /// <summary>
    /// Gets a hashcode for some <see cref="text"/>.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(scoped text text)
    {
        return Hasher.HashMany<char>(text);
    }
    
    /// <summary>
    /// Gets a hashcode for the contents of a <see cref="Span{T}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(scoped Span<T> span)
    {
        return Hasher.HashMany<T>(span);
    }
    
    /// <summary>
    /// Gets a hashcode for the contents of a <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(scoped ReadOnlySpan<T> span)
    {
        return Hasher.HashMany<T>(span);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Gets a hashcode for a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to hash, may be <c>null</c>.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> being hashed, <i>may</i> be a <c>ref struct</c>.
    /// </typeparam>
    /// <returns>
    /// An <see cref="int"/> hashcode of the <typeparamref name="T"/> <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// This method may not have to exist, as <c>ref struct</c> values cannot currently be stored in a HashSet nor Dictionary.
    /// But it does to allow for the possibility that more stack-based collections could exist in the future,
    /// and for maximum compatibility with <see cref="object"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return 0;
        return GetHashCodeCache<T>.GetHashCode(in value);
    }

    private static class GetHashCodeCache<T>
        where T : allows ref struct
    {
        private delegate int AnyGetHashCode(ref readonly T value);

        private volatile static AnyGetHashCode _delegate;
        private volatile static bool _delegateTested;

        static GetHashCodeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? getHashCodeMethod = instanceType.FindMatchingInstanceMethod("GetHashCode", typeof(int), []);

            if (getHashCodeMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyGetHashCode>($"Any_{instanceType}_GetHashCode");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, getHashCodeMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyGetHashCode>(out var func))
                {
                    _delegate = func;
                    _delegateTested = false;
                    return;
                }
            }

            _delegate = FallbackGetHashCode;
            _delegateTested = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FallbackGetHashCode(ref readonly T value) => Hasher.HashBytes<T>(in value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetHashCode(ref readonly T value)
        {
            if (_delegateTested)
            {
                return _delegate(in value);
            }

            return untestedInvoke(in value);
            
            static int untestedInvoke(ref readonly T value)
            {
                try
                {
                    return _delegate(in value);
                }
                catch
                {
                    _delegate = FallbackGetHashCode;
                    return _delegate(in value);
                }
                finally
                {
                    _delegateTested = true;
                }
            }
        }
    }
}


#endif