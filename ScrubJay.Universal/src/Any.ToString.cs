#if NET9_0_OR_GREATER
using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
#endif
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Returns a <see cref="string"/> representation of the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="string"/> representation of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="string"/> representation.
    /// </returns>
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? ToString<T>(in T? instance)
    {
        if (instance is null)
            return null;
        return instance.ToString()!;
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Returns a <see cref="string"/> representation of the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="string"/> representation of.
    /// </param>
    /// <param name="_">
    /// A <see cref="TypeConstraints"/> applied so that this method is only called with <see langword="ref struct"/> <paramref name="instance"/>s.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="string"/> representation.
    /// </returns>
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? ToString<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (instance is null)
            return null;
        return ToStringCache<T>.Invoke(in instance);
    }
    
    private static class ToStringCache<T>
        where T : allows ref struct
    {
        private delegate string AnyToString(ref readonly T value);

        private static volatile AnyToString _delegate;
        private static volatile bool _delegateTested;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods("ToString", typeof(string), Type.EmptyTypes)
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyToString>($"Any_{instanceType}_ToString");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyToString>(out var func))
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
        private static string Fallback(ref readonly T value)
            => typeof(T).ToString(); // same as object.ToString()

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string TryInvoke(ref readonly T value)
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
        public static string Invoke(ref readonly T value)
        {
            if (_delegateTested)
                return _delegate(in value);
            return TryInvoke(in value);
        }
    }
#endif

    public static string ToString<T>(scoped Span<T> span)
    {
        return span.ToString();
    }
    
    public static string ToString<T>(scoped ReadOnlySpan<T> span)
    {
        return span.ToString();
    }
}