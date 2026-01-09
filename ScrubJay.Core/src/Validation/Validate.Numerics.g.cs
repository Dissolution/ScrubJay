#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<byte> IsByte(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= byte.MaxValue && f64 >= byte.MinValue)
            return (byte)f64;
        return Ex.ArgRange(f64, $"was not a valid byte value [{byte.MinValue:N0}.. {byte.MaxValue:N0}]");
    }

    public static Result<uint> IsValidU32(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= uint.MaxValue && f64 >= uint.MinValue)
            return (uint)f64;
        return Ex.ArgRange(f64, $"was not a valid uint value [{uint.MinValue:N0}.. {uint.MaxValue:N0}]");
    }

    public static Result<ulong> IsValidU64(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= ulong.MaxValue && f64 >= ulong.MinValue)
            return (ulong)f64;
        return Ex.ArgRange(f64, $"was not a valid ulong value [{ulong.MinValue:N0}.. {ulong.MaxValue:N0}]");
    }
}
