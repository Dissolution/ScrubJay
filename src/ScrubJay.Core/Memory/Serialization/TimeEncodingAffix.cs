namespace ScrubJay.Memory.Serialization;

/// <summary>
/// Indicates a possible Prefix or Postfix associated with a sequence of bytes
/// that contains an offset from <c>0001-01-01 00:00:00.0</c>
/// </summary>
/// <seealso href="https://en.cppreference.com/w/c/chrono/time_t"/>
[PublicAPI]
public enum TimeEncodingAffix
{
    /// <summary>
    /// 100-nanosecond intervals (ticks) from origin
    /// </summary>
    /// <remarks>
    /// Size = 8 bytes
    /// </remarks>
    Ticks,

    /// <summary>
    /// <see cref="uint"/> seconds from origin
    /// </summary>
    /// <remarks>
    /// Size = 4 bytes
    /// </remarks>
    TimeU32,

    /// <summary>
    /// <see cref="ulong"/> seconds from origin
    /// </summary>
    /// <remarks>
    /// Size = 8 bytes
    /// </remarks>
    TimeU64,
}