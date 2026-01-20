namespace ScrubJay.Validation;

partial class Guard
{
    public static byte IsValidU8(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= byte.MaxValue && f64 >= byte.MinValue)
            return (byte)f64;
        throw Ex.ArgRange(f64, $"was not a valid byte value [{byte.MinValue:N0}..{byte.MaxValue:N0}]");
    }

    public static sbyte IsValidI8(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= sbyte.MaxValue && f64 >= sbyte.MinValue)
            return (sbyte)f64;
        throw Ex.ArgRange(f64, $"was not a valid sbyte value [{sbyte.MinValue:N0}..{sbyte.MaxValue:N0}]");
    }

    public static short IsValidI16(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= short.MaxValue && f64 >= short.MinValue)
            return (short)f64;
        throw Ex.ArgRange(f64, $"was not a valid short value [{short.MinValue:N0}..{short.MaxValue:N0}]");
    }
    
    public static ushort IsValidU16(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= ushort.MaxValue && f64 >= ushort.MinValue)
            return (ushort)f64;
        throw Ex.ArgRange(f64, $"was not a valid ushort value [{ushort.MinValue:N0}..{ushort.MaxValue:N0}]");
    }
    
    public static int IsValidI32(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= int.MaxValue && f64 >= int.MinValue)
            return (int)f64;
        throw Ex.ArgRange(f64, $"was not a valid int value [{int.MinValue:N0}..{int.MaxValue:N0}]");
    }
    
    public static uint IsValidU32(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= uint.MaxValue && f64 >= uint.MinValue)
            return (uint)f64;
        throw Ex.ArgRange(f64, $"was not a valid uint value [{uint.MinValue:N0}..{uint.MaxValue:N0}]");
    }

    public static long IsValidI64(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= long.MaxValue && f64 >= long.MinValue)
            return (long)f64;
        throw Ex.ArgRange(f64, $"was not a valid long value [{long.MinValue:N0}..{long.MaxValue:N0}]");
    }
    
    public static ulong IsValidU64(double f64,
        [CallerArgumentExpression(nameof(f64))]
        string? numberName = null)
    {
        if (f64 <= ulong.MaxValue && f64 >= ulong.MinValue)
            return (ulong)f64;
        throw Ex.ArgRange(f64, $"was not a valid ulong value [{ulong.MinValue:N0}..{ulong.MaxValue:N0}]");
    }
}