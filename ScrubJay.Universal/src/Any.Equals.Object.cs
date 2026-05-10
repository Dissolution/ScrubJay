#if NET9_0_OR_GREATER
using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
#endif
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    public static bool Equals<T>(in T? instance, object? other)
    {
        if (instance is null)
            return other is null;
        return instance.Equals(other);
    }

#if NET9_0_OR_GREATER

    public static bool Equals<T>(in T? instance, object? other, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (instance is null)
            return other is null;
        return EqualsObjectCache<T>.Invoke(in instance, other);
    }

    private static class EqualsObjectCache<T>
        where T : allows ref struct
    {
        private delegate bool AnyEqualsObject(ref readonly T value, object? other);

        private static volatile AnyEqualsObject _delegate;
        private static volatile bool _delegateTested;

        static EqualsObjectCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(object.Equals), typeof(bool), [typeof(object)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyEqualsObject>($"Any_{instanceType}_Equals_Object");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyEqualsObject>(out var func))
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
        private static bool Fallback(ref readonly T value, object? other) => false;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvoke(ref readonly T value, object? other)
        {
            try
            {
                return _delegate(in value, other);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(in value, other);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invoke(ref readonly T value, object? other)
        {
            if (_delegateTested)
                return _delegate(in value, other);
            return TryInvoke(in value, other);
        }
    }
#endif

    public static bool Equals<T>(scoped Span<T> span, object? other)
    {
        return false;
    }

    public static bool Equals<T>(scoped ReadOnlySpan<T> span, object? other)
    {
        return false;
    }
}