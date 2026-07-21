#pragma warning disable CS1574, CS1584, CS1581, CS1580
namespace ScrubJay.Reflection.Emission;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E"></typeparam>
/// <seealso cref="ILGenerator"/>
[PublicAPI]
public interface IExceptionEmitter<E> : IILEmitter<E>
    where E : IExceptionEmitter<E>
{
    /// <summary>
    /// Begins an <c>exception</c> block for a non-filtered exception.
    /// </summary>
    /// <param name="label">
    /// The <see cref="Label"/> for the end of the block.
    /// This will leave you in the correct place to execute <see langword="finally"/> blocks or to finish the <see langword="try"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// Creating an exception block records some information, but does not actually emit any Microsoft intermediate language (MSIL) onto the stream.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.beginexceptionblock"/>
    E BeginExceptionBlock(out Label label);

    /// <summary>
    /// Begins a <see langword="catch"/> block.
    /// </summary>
    /// <param name="exceptionType">
    /// The <see cref="Type"/> of <see cref="Exception"/> to <see langword="catch"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// Emits a branch instruction to the end of the current exception block.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The <see langword="catch"/> block is within a filtered exception.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="exceptionType"/> is <see langword="null"/>,
    /// and the exception filter block has not returned a value that indicates that <see langword="finally"/> blocks should be run until this <see langword="catch"/> block is located.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.begincatchblock"/>
    E BeginCatchBlock(Type? exceptionType);

    /// <summary>
    /// Begins a <see langword="catch"/> block.
    /// </summary>
    /// <typeparam name="X">
    /// The <see cref="Type"/> of <see cref="Exception"/> to <see langword="catch"/>.
    /// </typeparam>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The <see langword="catch"/> block is within a filtered exception.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.begincatchblock"/>
    E BeginCatchBlock<X>()
        where X : Exception;

    /// <summary>
    /// Ends an <c>exception</c> block.
    /// </summary>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// This operation occurs in an unexpected place in the code stream.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.endexceptionblock"/>
    E EndExceptionBlock();

    /// <summary>
    /// Begins an <c>exception</c> block for a filtered <see cref="Exception"/>.
    /// </summary>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// Emits a branch instruction to the end of the current exception block.
    /// If the current Emitter is associated with a <see cref="DynamicMethod"/> object, emitting filtered exception blocks is not supported.
    /// <see cref="DynamicILInfo"/> can be used to construct a dynamic method that uses filtered exception blocks.
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.<br/>
    /// <b>-or-</b><br/>
    /// This Emitter is emitting to a <see cref="DynamicMethod"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.beginexceptfilterblock"/>
    E BeginExceptFilterBlock();

    /// <summary>
    /// Begins an <c>exception</c> fault block in the Microsoft intermediate language (MSIL) stream.
    /// </summary>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// If the current Emitter is associated with a <see cref="DynamicMethod"/> object, emitting exception fault blocks is not supported.
    /// <see cref="DynamicILInfo"/> can be used to construct a dynamic method that uses exception fault blocks.
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.<br/>
    /// <b>-or-</b><br/>
    /// This Emitter is emitting to a <see cref="DynamicMethod"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.beginfaultblock"/>
    E BeginFaultBlock();

    /// <summary>
    /// Begins a <see langword="finally"/> block in the Microsoft intermediate language (MSIL) instruction stream.
    /// </summary>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The Emitter is not currently in an <c>exception</c> block.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.beginfinallyblock"/>
    E BeginFinallyBlock();
}