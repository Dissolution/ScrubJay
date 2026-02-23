using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Reflection;

namespace ScrubJay.Extensions;

[PublicAPI]
public static class ExceptionExtensions
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    internal extern static ref string? GetMessageField(this Exception ex);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paramName")]
    internal extern static ref string? GetParamNameField(this ArgumentException ex);
#else

    private delegate ref T GetFieldRef<in I, T>(I instance);

    private static readonly GetFieldRef<Exception, string?> _referenceExceptionMessageField =
        DynamicMethod.TryCreate<GetFieldRef<Exception, string?>>(
            "ref_Exception._message",
            gen =>
            {
                var field = typeof(Exception).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance).ThrowIfNull();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldflda, field);
                gen.Emit(OpCodes.Ret);
            }).OkOrThrow();

    private static readonly GetFieldRef<ArgumentException, string?> _referenceArgumentExceptionParamNameField =
        DynamicMethod.TryCreate<GetFieldRef<ArgumentException, string?>>(
            "ref_ArgumentException._paramName",
            gen =>
            {
                var field = typeof(ArgumentException).GetField(
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
            "_paramName",
#else
                    "m_paramName",
#endif
                    BindingFlags.NonPublic | BindingFlags.Instance).ThrowIfNull();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldflda, field);
                gen.Emit(OpCodes.Ret);
            }).OkOrThrow();

    internal static ref string? GetMessageField(this Exception ex) => ref _referenceExceptionMessageField(ex);

    internal static ref string? GetParamNameField(this ArgumentException ex) => ref _referenceArgumentExceptionParamNameField(ex);
#endif
}