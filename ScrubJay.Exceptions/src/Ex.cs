using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static partial class Ex
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    internal static extern ref string? RefMessageField(Exception exception);
#else
    private static readonly FieldInfo _exceptionMessageField = typeof(Exception).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance)!;

    private delegate ref string? RefMessageFieldDelegate(Exception exception);

    private static readonly RefMessageFieldDelegate _refExceptionMessageField;

    static Ex()
    {
        var method = DynamicMethod.New<RefMessageFieldDelegate>("Exception._message");
        var gen = method.GetILGenerator();
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Ldflda, _exceptionMessageField);
        gen.Emit(OpCodes.Ret);
        _refExceptionMessageField = method.CreateDelegate<RefMessageFieldDelegate>();
    }

    public static ref string? RefMessageField(Exception exception) => ref _refExceptionMessageField(exception);

#endif


}