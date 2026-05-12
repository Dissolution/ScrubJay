using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance)
        where T : ISpanParsable<T>
    {
#if NET7_0_OR_GREATER
        return T.TryParse(text, provider, out instance);
#else
        return TryParseSpanCache<T>.Invoke(text, provider, out instance);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        return TryParseSpanCache<T>.Invoke(text, provider, out instance);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return TryParseSpanCache<T>.Invoke(text, provider, out instance);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        [MaybeNullWhen(false)] out T instance)
        where T : ISpanParsable<T>
        => TryParse<T>(text, null, out instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.IsUnconstrained<T> _ = default)
        => TryParse<T>(text, null, out instance, _);

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return TryParse<T>(text, null, out instance, _);
    }
#endif

// ------------------------------


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance)
        where T : IParsable<T>
    {
#if NET7_0_OR_GREATER
        return T.TryParse(str, provider, out instance);
#else
        return TryParseCache<T>.Invoke(str, provider, out instance);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        return TryParseCache<T>.Invoke(str, provider, out instance);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return TryParseCache<T>.Invoke(str, provider, out instance);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        [MaybeNullWhen(false)] out T instance)
        where T : IParsable<T>
        => TryParse<T>(str, null, out instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.IsUnconstrained<T> _ = default)
        => TryParse<T>(str, null, out instance, _);

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        [AllowNull, NotNullWhen(true)] string? str,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => TryParse<T>(str, null, out instance, _);
#endif



// ------------------------------

    private static class TryParseCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate bool TryParse([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance);

        private static volatile TryParse _delegate;
        private static volatile bool _delegateTested;

        static TryParseCache()
        {
            Type type = typeof(T);
            MethodInfo? method = type
                .FindMatchingStaticMethods("TryParse",
                    typeof(bool),
                    [typeof(string), typeof(IFormatProvider), type.MakeByRefType()])
                .FirstOrDefault();

            if (method is not null)
            {
                Debug.Assert(method.IsStatic);

                var dynamicMethod = DynamicMethod.New<TryParse>($"{type}_TryParse");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<TryParse>(out var func))
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
        private static bool Fallback([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            instance = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvoke([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            try
            {
                return _delegate(str, provider, out instance);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(str, provider, out instance);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invoke([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            if (_delegateTested)
                return _delegate(str, provider, out instance);
            return TryInvoke(str, provider, out instance);
        }
    }


    private static class TryParseSpanCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate bool TryParseSpan(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance);

        private static volatile TryParseSpan _delegate;
        private static volatile bool _delegateTested;

        static TryParseSpanCache()
        {
            Type type = typeof(T);
            MethodInfo? method = type
                .FindMatchingStaticMethods("TryParse",
                    typeof(bool),
                    [typeof(text), typeof(IFormatProvider), type.MakeByRefType()])
                .FirstOrDefault();

            if (method is not null)
            {
                Debug.Assert(method.IsStatic);

                var dynamicMethod = DynamicMethod.New<TryParseSpan>($"{type}_TryParseSpan");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<TryParseSpan>(out var func))
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
        private static bool Fallback(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            instance = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvoke(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            try
            {
                return _delegate(text, provider, out instance);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(text, provider, out instance);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invoke(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            if (_delegateTested)
                return _delegate(text, provider, out instance);
            return TryInvoke(text, provider, out instance);
        }
    }

}