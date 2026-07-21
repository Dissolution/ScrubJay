#pragma warning disable CS1574

namespace ScrubJay.Reflection.Emission;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E"></typeparam>
/// <remarks>
/// If the <see cref="OpCode"/> parameter requires an argument,
/// the caller <b>must</b> ensure that the argument length matches the length of the declared parameter.<br/>
/// Otherwise, results will be unpredictable:<br/>
/// <i>For example,
/// if the <see cref="OpCode"/> requires a 2-<see cref="byte"/> argument and the caller supplies a 4-<see cref="byte"/> operand,
/// the runtime will emit two additional <see cref="byte">bytes</see> to the instruction stream.
/// These extra bytes will be <see cref="OpCodes.Nop"/> instructions.
/// </i>
/// </remarks>
/// <seealso cref="ILGenerator"/>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit"/>
[PublicAPI]
public interface IOpCodeEmitter<E> : IILEmitter<E>
    where E : IOpCodeEmitter<E>
{

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> onto the stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode)"/>
    E Emit(OpCode opcode);

    /// <summary>
    /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="u8">
    /// The character argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, byte u8);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> and <see cref="sbyte"/> argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="i8">
    /// The <see cref="sbyte"/> argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-sbyte)"/>
    E Emit(OpCode opcode, sbyte i8);

    /// <summary>
    /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="i16">
    /// The Int argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, short i16);

    /// <summary>
    /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="i32">
    /// The numerical argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, int i32);

    /// <summary>
    /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="i64">
    /// The numerical argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, long i64);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> and <see cref="float"/> argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="f32">
    /// The <see cref="float"/> argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-single)"/>
    E Emit(OpCode opcode, float f32);

    /// <summary>
    /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="f64">
    /// The numerical argument pushed onto the stream immediately after the instruction.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, double f64);

    /// <summary>
    /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="method">
    /// A <see cref="MethodInfo"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The location of <paramref name="method"/> is recorded so that the instruction stream can be patched if necessary when persisting the module to a portable executable (PE) file.
    /// If <paramref name="method"/> represents a generic method, it must be a generic method definition.
    /// That is, its <see cref="MethodInfo.IsGenericMethodDefinition"/> property must be <see langword="true"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="method"/> is a generic method for which <see cref="MethodInfo.IsGenericMethodDefinition"/> is <see langword="false"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, MethodInfo method);

    /// <summary>
    /// Puts the specified instruction and a signature token onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="signature">
    /// A <see cref="SignatureHelper"/> for constructing a signature token.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="signature"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-emit-signaturehelper)"/>
    E Emit(OpCode opcode, SignatureHelper signature);

    /// <summary>
    /// Puts the specified instruction and metadata token for the specified constructor onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="constructor">
    /// A <see cref="ConstructorInfo"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The location of <paramref name="constructor"/> is recorded so that the instruction stream can be patched if necessary when persisting the module to a portable executable (PE) file.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="constructor"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, ConstructorInfo constructor);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> onto the Microsoft intermediate language (MSIL) stream
    /// followed by the metadata token for the given <see cref="Type"/>.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="type">
    /// A <see cref="Type"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The location of <paramref name="type"/> is recorded so that the token can be patched if necessary when persisting the module to a portable executable (PE) file.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-type)"/>
    E Emit(OpCode opcode, Type type);

    /// <summary>
    /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="label">
    /// The label to which to branch from this location.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// Labels are created using DefineLabel, and their location within the stream is fixed by using MarkLabel. If a single-byte instruction is used, the label can represent a jump of at most 127 bytes along the stream. opcode must represent a branch instruction. Because branches are relative instructions, label will be replaced with the correct offset to branch during the fixup process.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-methodinfo)"/>
    E Emit(OpCode opcode, Label label);

    /// <summary>
    /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="labels">
    /// The array of label objects to which to branch from this location. All of the labels will be used.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// Emits a switch table.
    /// Labels are created using DefineLabel and their location within the stream is fixed by using MarkLabel.
    /// If a single-byte instruction is used, the label can represent a jump of at most 127 bytes along the stream.
    /// <paramref name="opcode"/> must represent a branch instruction.
    /// Because branches are relative instructions, label will be replaced with the correct offset to branch during the fixup process.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="labels"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-emit-label())"/>
    E Emit(OpCode opcode, Label[] labels);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> and metadata token for the specified <see cref="FieldInfo"/>
    /// onto the Microsoft intermediate language (MSIL) stream of instructions.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="field">
    /// A <see cref="FieldInfo"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The location of field is recorded so that the instruction stream can be patched if necessary when persisting the module to a portable executable (PE) file.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="field"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-fieldinfo)"/>
    E Emit(OpCode opcode, FieldInfo field);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> onto the Microsoft intermediate language (MSIL) stream
    /// followed by the metadata token for the given <see cref="string"/>.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="str">
    /// The <see cref="string"/> to be emitted.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The location of <paramref name="str"/> is recorded for future fixups if the module is persisted to a portable executable (PE) file.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="str"/> is <see langword="null"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-string)"/>
    E Emit(OpCode opcode, string str);

    /// <summary>
    /// Puts the specified <see cref="OpCode"/> onto the Microsoft intermediate language (MSIL) stream
    /// followed by the index of the given <paramref name="local"/> variable.
    /// </summary>
    /// <param name="opcode">
    /// The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.
    /// </param>
    /// <param name="local">
    /// A Declared <see cref="LocalBuilder"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The parent method of the <paramref name="local"/> does not match the method associated with this Emitter.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="local"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="opcode"/> is a single-<see cref="byte"/> instruction,
    /// and <paramref name="local"/> represents a local variable with an index greater than <see cref="byte.MaxValue"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.emit#system-reflection-emit-ilgenerator-emit(system-reflection-emit-opcode-system-reflection-emit-localbuilder)"/>
    E Emit(OpCode opcode, LocalBuilder local);
}