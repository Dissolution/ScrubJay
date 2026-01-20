#pragma warning disable CA1040, CA1716

namespace ScrubJay;

#if NET7_0_OR_GREATER
/// <summary>
/// Indicates that this type provides a <c>static</c> <see cref="Default"/> instance
/// </summary>
/// <typeparam name="S">
/// <c>typeof(self)</c>
/// </typeparam>
[PublicAPI]
public interface IHasDefault<out S>
    where S : IHasDefault<S>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{

    /// <summary>
    /// Get the <c>default</c> <typeparamref name="S"/> instance
    /// </summary>
    static abstract S Default { get; }
}
#else
/// <summary>
/// Indicates that this type provides a <c>static</c> <c>default</c> instance of itself
/// </summary>
/// <typeparam name="S">
/// <c>typeof(self)</c>
/// </typeparam>
[PublicAPI]
public interface IHasDefault<out S>
    where S : IHasDefault<S>
{

}
#endif
