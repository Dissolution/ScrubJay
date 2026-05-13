#if NET9_0_OR_GREATER
using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
#endif
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<TEquatable>(in TEquatable? left, in TEquatable? right)
        where TEquatable : IEquatable<TEquatable>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
            return left.Equals(right!);
        if (right is not null)
            return right.Equals(left!);
        return true;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? left, in T? right,
        TypeConstraints.IsClass<T> _ = default)
        where T : class
    {
        return EqualityComparer<T>.Default.Equals(left!, right!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T left, in T right,
        TypeConstraints.IsStruct<T> _ = default)
        where T : struct
    {
        return EqualityComparer<T>.Default.Equals(left, right);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T left, in T right,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : struct, allows ref struct
    {
        return EqualsCache<T>.Invoke(in left, in right);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? left, in T? right,
        IEqualityComparer<T>? comparer,
        TypeConstraints.IsClass<T> _ = default)
        where T : class
    {
        if (comparer is null)
            return EqualityComparer<T>.Default.Equals(left!, right!);
        return comparer.Equals(left!, right!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T left, in T right,
        IEqualityComparer<T>? comparer,
        TypeConstraints.IsStruct<T> _ = default)
        where T : struct
    {
        if (comparer is null)
            return EqualityComparer<T>.Default.Equals(left, right);
        return comparer.Equals(left, right);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T left, in T right,
        IEqualityComparer<T>? comparer,
        TypeConstraints.IsStructAllowsRefStruct<T> _ = default)
        where T : struct, allows ref struct
    {
        if (comparer is null)
            return EqualsCache<T>.Invoke(in left, in right);
        return comparer.Equals(left, right);
    }
#endif


#if NET9_0_OR_GREATER
    private static class EqualsCache<T>
        where T : allows ref struct
    {
        private delegate bool AnyEquals(ref readonly T left, ref readonly T? right);

        private static volatile AnyEquals _delegate;
        private static volatile bool _delegateTested;

        static EqualsCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(IEquatable<>.Equals), typeof(bool), [instanceType])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyEquals>($"Any_{instanceType}_Equals");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldobj, instanceType);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyEquals>(out var func))
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
        private static bool Fallback(ref readonly T left, ref readonly T? right)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ceq();
            return Return<bool>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvoke(ref readonly T left, ref readonly T? right)
        {
            try
            {
                return _delegate(in left, in right);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(in left, in right);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invoke(ref readonly T left, ref readonly T? right)
        {
            if (_delegateTested)
                return _delegate(in left, in right);
            return TryInvoke(in left, in right);
        }
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(scoped ReadOnlySpan<T> left, scoped ReadOnlySpan<T> right)
        where T : IEquatable<T>
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.SequenceEqual(left, right);
    }

#if NET6_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool Equals<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
#if NET6_0_OR_GREATER
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.SequenceEqual(left, right);
#else
        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(scoped text left, scoped text right)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(scoped text left, scoped text right, StringComparison comparison)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.Equals(left, right, comparison);
    }
}