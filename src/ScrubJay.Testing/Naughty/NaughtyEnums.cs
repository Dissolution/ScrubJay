#pragma warning disable CA1008, CA1028, CA1069, CA1711

namespace ScrubJay.Testing.Naughty;

[PublicAPI]
public static class NaughtyEnums
{
    [Flags]
    public enum NaughtyPermissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
    }

    public enum DuplicateNames
    {
        Primary = 1,
        Alias = 1,
    }

    public enum NoDefinedDefault
    {
        Alpha = 1,
        Beta = 2,
        Gamma = 3,
        Delta = 4,
    }

    public enum ComplexLongEnum : long
    {
        Min = long.MinValue,
        NegOne = -1L,
        Zero = 0L,
        PlusOne = +1L,
        Max = long.MaxValue,
    }

    public enum ByteEnum : byte
    {
        Zero = 0,
        PlusOne = 1,
        Max = byte.MaxValue,
    }
}