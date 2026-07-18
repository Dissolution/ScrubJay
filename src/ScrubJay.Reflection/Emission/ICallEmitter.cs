namespace ScrubJay.Reflection.Emission;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E"></typeparam>
/// <seealso cref="ILGenerator"/>
[PublicAPI]
public interface ICallEmitter<E> : IILEmitter<E>
    where E : ICallEmitter<E>
{
    /// <inheritdoc cref="ILGenerator.EmitCalli(OpCode,CallingConvention, Type, Type[])"/>
    E EmitCalli(
        CallingConvention unmanagedCallConv,
        Type? returnType,
        Type[]? parameterTypes);

    /// <inheritdoc cref="ILGenerator.EmitCalli(OpCode,CallingConventions, Type, Type[], Type[])"/>
    E EmitCalli(
        CallingConventions callingConvention,
        Type? returnType,
        Type[]? parameterTypes,
        Type[]? optionalParameterTypes = null);

    /// <inheritdoc cref="ILGenerator.EmitCall"/>
    E EmitCall(OpCode opcode, MethodInfo methodInfo, Type[]? optionalParameterTypes);
}