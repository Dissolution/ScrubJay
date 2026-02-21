namespace ScrubJay.Validation;

partial class Ex
{
    /// <summary>
    /// Get a new <see cref="InvalidOperationException"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid() => new();

    /// <summary>
    /// Get a new <see cref="InvalidOperationException"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid(string? message) => new();

    /// <summary>
    /// Get a new <see cref="InvalidOperationException"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid(DefaultInterpolatedStringHandler info) => new();
}