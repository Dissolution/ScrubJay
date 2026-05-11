using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [return: NotNullIfNotNull(nameof(instance))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? Format<F>(
        in F? instance,
        string? format = null,
        IFormatProvider? formatProvider = null)
        where F : IFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is null)
            return null;
        return instance.ToString(format, formatProvider);
    }

    [return: NotNullIfNotNull(nameof(instance))]
    public static string? Format<T>(
        in T? instance,
        string? format = null,
        IFormatProvider? formatProvider = null,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        if (instance is null)
            return null;
        return CompareCache
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T>(
        in T? instance,
        string? format = null,
        IFormatProvider? formatProvider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
   
    }
#endif

    private static class FormatCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate string AnyFormat(ref readonly T instance, string? format, IFormatProvider? provider);

        private static volatile AnyFormat _delegate;
        private static volatile bool _delegateTested;

        static FormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(IFormattable.ToString), typeof(string), [typeof(string), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyFormat>($"Any_{instanceType}_Format");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyFormat>(out var func))
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
        private static string Fallback(ref readonly T instance, string? format, IFormatProvider? provider)
        {
            return Any.ToString<T>(in instance)!;
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
    public static string Format<C>(scoped ReadOnlySpan<C> left, scoped ReadOnlySpan<C> right)
        where C : IComparable<C>
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.SequenceCompareTo(left, right);
    }

    public static string Format<T>(
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


    public static string Format<T>(
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
    public static string Format(scoped text left, scoped text right)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.CompareTo(left, right, StringComparison.Ordinal);
    }

    public static string Format(scoped text left, scoped text right, StringComparison comparison)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return MemoryExtensions.CompareTo(left, right, comparison);
    }
}