#pragma warning disable CS1574, CS1584, CS1581, CS1580
namespace ScrubJay.Reflection.Emission;

/// <summary>
/// 
/// </summary>
/// <typeparam name="E"></typeparam>
/// <seealso cref="ILGenerator"/>
[PublicAPI]
public interface IInternalsEmitter<E> : IILEmitter<E>
    where E : IInternalsEmitter<E>
{
    int ILOffset { get; }

    /// <summary>
    /// Declares a new label.
    /// </summary>
    /// <param name="label">
    /// A new <see cref="Label"/> that can be used as a token for branching.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// To set the position of the label within the stream, you must call <see cref="MarkLabel"/>.<br/>
    /// Failure to do so will cause an <see cref="ArgumentException"/> when <see cref="TypeBuilder.CreateType"/> is called.<br/>
    /// This is just a token and does not <i>yet</i> represent any particular location within the stream.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.definelabel"/>
    E DefineLabel(out Label label);
    
    
    E MarkLabel(Label label);

    /// <summary>
    /// Declares a local variable of the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="localType">
    /// The <see cref="Type"/> of the local variable.
    /// </param>
    /// <param name="local">
    /// The declared <see cref="LocalBuilder"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The local variable is created in the current lexical scope;<br/>
    /// <i>for example, if code is being emitted in a <see langword="for"/> loop, the scope of the variable </i>is<i> the loop.</i><br/>
    /// A local variable created with this overload is not pinned.<br/>
    /// To create a pinned variable for use with unmanaged pointers, use <see cref="DeclareLocal(Type, bool, out LocalBuilder)"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="localType"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="localType"/> was created by <see cref="TypeBuilder.CreateType"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.declarelocal"/>
    E DeclareLocal(Type localType, out LocalBuilder local);

    /// <summary>
    /// Declares a local variable of the specified <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="L">
    /// The <see cref="Type"/> of the local variable.
    /// </typeparam>
    /// <param name="local">
    /// The declared <see cref="LocalBuilder"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The local variable is created in the current lexical scope;<br/>
    /// <i>for example, if code is being emitted in a <see langword="for"/> loop, the scope of the variable </i>is<i> the loop.</i><br/>
    /// A local variable created with this overload is not pinned.<br/>
    /// To create a pinned variable for use with unmanaged pointers, use <see cref="DeclareLocal{T}(bool, out LocalBuilder)"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="L"/> was created by <see cref="TypeBuilder.CreateType"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.declarelocal"/>
    E DeclareLocal<L>(out LocalBuilder local)
#if NET9_0_OR_GREATER
        where L : allows ref struct
#endif
    ;
    
    /// <summary>
    /// Declares a local variable of the specified <see cref="Type"/>,
    /// optionally pinning the object referred to by the variable.
    /// </summary>
    /// <param name="localType">
    /// The <see cref="Type"/> of the local variable.
    /// </param>
    /// <param name="pinned">
    /// <see langword="true"/> to pin the object in memory; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="local">
    /// The declared <see cref="LocalBuilder"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The local variable is created in the current lexical scope;<br/>
    /// <i>for example, if code is being emitted in a <see langword="for"/> loop, the scope of the variable </i>is<i> the loop.</i><br/>
    /// In <see langword="unsafe"/> code, an object must be pinned before it can be referred to by an unmanaged pointer.<br/>
    /// While the referenced object is pinned, it cannot be moved by garbage collection.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="localType"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="localType"/> was created by <see cref="TypeBuilder.CreateType"/>.<br/>
    /// <b>-or-</b><br/>
    /// The method body of the enclosing method was created by <see cref="MethodBuilder.CreateMethodBody(byte[], int)"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The method with which this Emitter is associated is not represented by a <see cref="MethodBuilder"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.declarelocal"/>
    E DeclareLocal(Type localType, bool pinned, out LocalBuilder local);
    
    /// <summary>
    /// Declares a local variable of the specified <see cref="Type"/>,
    /// optionally pinning the object referred to by the variable.
    /// </summary>
    /// <typeparam name="L">
    /// The <see cref="Type"/> of the local variable.
    /// </typeparam>
    /// <param name="pinned">
    /// <see langword="true"/> to pin the object in memory; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="local">
    /// The declared <see cref="LocalBuilder"/>.
    /// </param>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// The local variable is created in the current lexical scope;<br/>
    /// <i>for example, if code is being emitted in a <see langword="for"/> loop, the scope of the variable </i>is<i> the loop.</i><br/>
    /// In <see langword="unsafe"/> code, an object must be pinned before it can be referred to by an unmanaged pointer.<br/>
    /// While the referenced object is pinned, it cannot be moved by garbage collection.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="L"/> was created by <see cref="TypeBuilder.CreateType"/>.<br/>
    /// <b>-or-</b><br/>
    /// The method body of the enclosing method was created by <see cref="MethodBuilder.CreateMethodBody(byte[], int)"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The method with which this Emitter is associated is not represented by a <see cref="MethodBuilder"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.declarelocal"/>
    E DeclareLocal<L>(bool pinned, out LocalBuilder local);


    E UsingNamespace(string @namespace);

    /// <summary>
    /// Begins a lexical scope.
    /// </summary>
    /// <returns>
    /// A fluent reference to this <typeparamref name="E"/>.
    /// </returns>
    /// <remarks>
    /// This method is used to emit symbolic information.
    /// Local variables declared after <see cref="BeginScope"/> are scoped until the corresponding <see cref="EndScope"/> is called.
    /// If the current Emitter is associated with a <see cref="DynamicMethod"/>, it does not support symbolic information.
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// The current Emitter is associated with a <see cref="DynamicMethod"/>.
    /// </exception>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator.beginscope"/>
    E BeginScope();
    
    E EndScope();
}