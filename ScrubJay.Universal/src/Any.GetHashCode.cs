#if NET9_0_OR_GREATER
using System.Reflection;
using System.Reflection.Emit;
#endif
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Returns a <see cref="int"/> hashcode for the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to get the hashcode of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s hashcode or <c>0</c> if the <paramref name="instance"/> is <see langword="null"/>.
    /// </returns>
    public static int GetHashCode<T>(in T? instance)
    {
        if (instance is null)
            return 0;
        return instance.GetHashCode();
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Returns a <see cref="int"/> hashcode for the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to get the hashcode of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s hashcode or <c>0</c> if the <paramref name="instance"/> is <see langword="null"/>.
    /// </returns>
    public static int GetHashCode<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (instance is null)
            return 0;
        return GetHashCodeCache<T>.Invoke(in instance);
    }

    private static class GetHashCodeCache<T>
        where T : allows ref struct
    {
        private delegate int AnyGetHashCode(ref readonly T value);

        private static volatile AnyGetHashCode _delegate;
        private static volatile bool _delegateTested;

        static GetHashCodeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods("GetHashCode", typeof(int), Type.EmptyTypes)
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyGetHashCode>($"Any_{instanceType}_GetHashCode");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyGetHashCode>(out var func))
                {
                    _delegate = func;
                    _delegateTested = false;
                    return;
                }
            }

            _delegate = Fallback;
            _delegateTested = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int Fallback(ref readonly T value)
            => Hasher.HashReferenceBytes(in value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int TryInvoke(ref readonly T value)
        {
            try
            {
                return _delegate(in value);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(in value);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Invoke(ref readonly T value)
        {
            if (_delegateTested)
                return _delegate(in value);
            return TryInvoke(in value);
        }
    }
#endif

    public static int GetHashCode<T>(scoped Span<T> span)
    {
        return Hasher.HashMany(span);
    }

    public static int GetHashCode<T>(scoped ReadOnlySpan<T> span)
    {
        return Hasher.HashMany(span);
    }

    public static int GetHashCode(scoped ReadOnlySpan<char> text)
    {
#if NET6_0_OR_GREATER
        return string.GetHashCode(text, StringComparison.Ordinal);
#else
        return Hasher.HashMany<char>(text);
#endif
    }
}