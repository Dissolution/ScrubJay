using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

public partial class Any
{

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

                var dynamicMethod = CreateDynamicMethod<TryParse>($"{type}_TryParse");
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
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
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

}