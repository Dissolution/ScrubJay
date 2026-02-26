namespace ScrubJay.Sandboxes;

public static class TestType
{
    public sealed class SealedClass;

    public abstract class AbstractClass;

    public sealed record class SealedRecordClass(int Id, string? Name);


    public struct Struct
    {
        public override string ToString() => $"{nameof(TestType)}.{nameof(Struct)}.{nameof(ToString)}()";
    }

    public readonly struct ReadonlyStruct;

    public ref struct RefStruct
    {
        public override string ToString() => $"{nameof(TestType)}.{nameof(RefStruct)}.{nameof(ToString)}()";
    }

    public readonly ref struct ReadonlyRefStruct
    {
        public override string ToString() => $"{nameof(TestType)}.{nameof(RefStruct)}.{nameof(ToString)}()";
    }

    public record struct RecordStruct(int Id, string? Name);

    public readonly record struct ReadonlyRecordStruct(int Id, string? Name);


    public enum TestEnum
    {
        None,
        Alfa,
        Bravo,
        Charlie,
        Delta,
    }

    [Flags]
    public enum TestFlaggedEnum
    {
        None = 0,
        Alpha = 1 << 0,
        Beta = 1 << 1,
        Gamma = 1 << 2,
        Delta = 1 << 3,
    }
}