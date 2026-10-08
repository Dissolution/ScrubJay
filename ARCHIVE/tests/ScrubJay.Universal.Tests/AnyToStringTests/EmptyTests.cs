using ScrubJay.Testing.Things.Empty;
using ScrubJay.Testing.Validation;

namespace ScrubJay.Universal.Tests.AnyToStringTests;

public class EmptyTests
{
#if NET9_0_OR_GREATER
    [Fact]
    public void EmptyRefStructThingWorks()
    {
        EmptyRefStructThing thing = new();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString).IsNotNull();
    }

    [Fact]
    public void EmptyReadonlyRefStructThingWorks()
    {
        EmptyReadonlyRefStructThing thing = new();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString).IsNotNull();
    }
#endif

    [Fact]
    public void EmptyStructThingWorks()
    {
        EmptyStructThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyReadonlyStructThingWorks()
    {
        EmptyReadonlyStructThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyRecordStructThingWorks()
    {
        EmptyRecordStructThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyReadonlyRecordStructThingWorks()
    {
        EmptyReadonlyRecordStructThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyClassThingWorks()
    {
        EmptyClassThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptySealedClassThingWorks()
    {
        EmptySealedClassThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyClassSuperThingWorks()
    {
        EmptyClassSuperThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptySealedClassSuperThingWorks()
    {
        EmptySealedClassSuperThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyRecordClassThingWorks()
    {
        EmptyRecordClassThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptySealedRecordClassThingWorks()
    {
        EmptySealedRecordClassThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyRecordClassSuperThingWorks()
    {
        EmptyRecordClassSuperThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

    [Fact]
    public void EmptyRecordSealedClassSuperThingWorks()
    {
        EmptyRecordSealedClassSuperThing thing = new();
        string? toString = thing.ToString();
        string? anyToString = Any.ToString(in thing);
        Demand.That(anyToString)
            .IsNotNull()
            .IsEqualTo(toString);
    }

}