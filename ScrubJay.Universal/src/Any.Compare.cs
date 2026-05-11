using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<C>(in C? instance, in C? other)
        where C : IComparable<C>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is not null)
            return instance.CompareTo(other!);

        if (other is not null)
            return -1;

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<C>(in C? instance, object? other, TypeConstraints.HasIComparable _ = default)
        where C : IComparable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is not null)
            return instance.CompareTo(other!);

        if (other is not null)
            return -1;

        return 0;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right, IComparer<T>? comparer)
    {
        if (comparer is null)
            return Compare<T>(in left, in right, default(TypeConstraints.IsUnconstrained<T>));
        return comparer.Compare(left!, right!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right, TypeConstraints.IsUnconstrained<T> _ = default)
    {
        if (left is not null)
            return CompareCache<T>.Invoke(in left, in right);

        if (right is not null)
            return -1;

        return 0;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right,
        IComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (comparer is null)
            return Compare<T>(in left, in right, _);
        return comparer.Compare(left!, right!);
    }
#endif

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (left is not null)
            return CompareCache<T>.Invoke(in left, in right);

        if (right is not null)
            return -1;

        return 0;
    }
#endif

    private static class CompareCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate int AnyCompare(ref readonly T left, ref readonly T? right);

        private static volatile AnyCompare _delegate;
        private static volatile bool _delegateTested;

        static CompareCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(IComparable<>.CompareTo), typeof(int), [instanceType])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyCompare>($"Any_{instanceType}_CompareTo");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldobj, instanceType);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyCompare>(out var func))
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
        private static int Fallback(ref readonly T left, ref readonly T? right)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Clt();
            Emit.Brtrue("lt");
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Cgt();
            Emit.Brtrue("gt");
            Emit.Ldc_I4_0();
            Emit.Ret();
            MarkLabel("lt");
            Emit.Ldc_I4_M1();
            Emit.Ret();
            MarkLabel("gt");
            Emit.Ldc_I4_1();
            Emit.Ret();
            throw Unreachable();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int TryInvoke(ref readonly T left, ref readonly T? right)
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
        public static int Invoke(ref readonly T left, ref readonly T? right)
        {
            if (_delegateTested)
                return _delegate(in left, in right);
            return TryInvoke(in left, in right);
        }
    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<C>(scoped ReadOnlySpan<C> left, scoped ReadOnlySpan<C> right)
        where C : IComparable<C>
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.SequenceCompareTo(left, right);
    }
    
    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        int minLength = Math.Min(left.Length, right.Length);
        
        for (int i = 0; i < minLength; i++)
        {
            int c = Compare(left[i], right[i], _);
            if (c != 0)
            {
                return c;
            }
        }

        return left.Length.CompareTo(right.Length);
    }

    
    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IComparer<T>? comparer)
    {
        if (comparer is null)
            return Compare<T>(left, right, default(TypeConstraints.IsUnconstrained<T>));
        
        int minLength = Math.Min(left.Length, right.Length);
        
        for (int i = 0; i < minLength; i++)
        {
            int c = comparer.Compare(left[i], right[i]);
            if (c != 0)
            {
                return c;
            }
        }

        return left.Length.CompareTo(right.Length);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare(scoped text left, scoped text right)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.CompareTo(left, right, StringComparison.Ordinal);
    }

    public static int Compare(scoped text left, scoped text right, StringComparison comparison)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.CompareTo(left, right, comparison);
    }
}