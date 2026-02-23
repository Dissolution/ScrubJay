namespace ScrubJay.Tests.EnumTests;

public static class TestEnums
{

    public enum TestEnumI8 : sbyte
    {
        Min = sbyte.MinValue,
        NegOne = -1,
        Zero = 0,
        One = 1,
        Max = sbyte.MaxValue,
    }

    public enum TestEnumU8 : byte
    {
        Min = byte.MinValue,
        Zero = 0,
        One = 1,
        Max = byte.MaxValue,
    }

    public enum TestEnumI16 : short
    {
        Min = short.MinValue,
        NegOne = -1,
        Zero = 0,
        One = 1,
        Max = short.MaxValue,
    }

    public enum TestEnumU16 : ushort
    {
        Min = ushort.MinValue,
        Zero = 0,
        One = 1,
        Max = ushort.MaxValue,
    }

    public enum TestEnumI32 : int
    {
        Min = int.MinValue,
        NegOne = -1,
        Zero = 0,
        One = 1,
        Max = int.MaxValue,
    }

    public enum TestEnumU32 : uint
    {
        Min = uint.MinValue,
        Zero = 0,
        One = 1,
        Max = uint.MaxValue,
    }

    public enum TestEnumI64 : long
    {
        Min = long.MinValue,
        NegOne = -1L,
        Zero = 0L,
        One = 1L,
        Max = long.MaxValue,
    }

    public enum TestEnumU64 : ulong
    {
        Min = ulong.MinValue,
        Zero = 0,
        One = 1,
        Max = ulong.MaxValue,
    }

    [Flags]
    public enum TestEnumFlagsI8 : sbyte
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
    }

    [Flags]
    public enum TestEnumFlagsU8 : byte
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsI16 : short
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsU16 : ushort
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsI32 : int
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsU32 : uint
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsI64 : long
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }

    [Flags]
    public enum TestEnumFlagsU64 : ulong
    {
        None = 0,
        Alfa = 1 << 0,
        Bravo = 1 << 1,
        Charlie = 1 << 2,
        Delta = 1 << 3,
        Echo = 1 << 4,
        Foxtrot = 1 << 5,
        Golf = 1 << 6,
        Hotel = 1 << 7,
    }



    public enum TestEnumNegativeI32 : int
    {
        NegThree = -3,
        NegTwo = -2,
        NegOne = -1,
        Zero = 0,
    }

    public enum TestEnumSparseI32 : int
    {
        None = 0,
        OK = 200,
        NotFound = 404,
        ServerError = 500,
    }

    public enum TestEnumNoMembersI32 : int
    {

    }

    public enum TestEnumOneMemberI32 : int
    {
        Only = 0,
    }

    public enum TestEnumNoZeroI32 : int
    {
        One = 1,
        Two = 2,
        Three = 3,
    }
}