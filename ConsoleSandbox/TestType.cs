namespace ScrubJay.Sandboxes;

public static class TestType
{
    public sealed class SealedClass;

    public abstract class AbstractClass;

    public sealed record class SealedRecordClass(int Id, string? Name);

    public struct Struct;

    public readonly struct ReadonlyStruct;

    public record struct RecordStruct(int Id, string? Name);

    public ref struct RefStruct
    {
        public override string ToString() => "RefStruct.ToString";
    }
}