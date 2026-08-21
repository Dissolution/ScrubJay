namespace ScrubJay.Functional;

/// <summary>
/// The <see cref="Unit"/> type is like <see langword="void"/> in that it indicates the absence of a value.<br/>
/// <see cref="Unit"/> only has a single instance and can act as a placeholder when no value exists or is needed.
/// </summary>
/// <remarks>
/// This is especially useful in generic <see langword="Type">Types</see> and <see langword="Method">Methods</see> where <see langword="void"/> cannot be used.
/// </remarks>
[PublicAPI]
[StructLayout(LayoutKind.Auto, Size = 0)]
public readonly struct Unit :
#if NET7_0_OR_GREATER
    IEqualityOperators<Unit, Unit, bool>,
    IComparisonOperators<Unit, Unit, bool>,
#endif
    IEquatable<Unit>,
    IComparable<Unit>
{
    // Treat ValueTuple like Unit -- the T0 instance cannot be created via syntax, but it would be `()`, the same as Unit
    public static implicit operator Unit(ValueTuple _) => default;
    public static implicit operator ValueTuple(Unit _) => default;

    // All units are exactly the same
    public static bool operator ==(Unit _, Unit __) => true;
    public static bool operator !=(Unit _, Unit __) => false;
    public static bool operator >(Unit _, Unit __) => false;
    public static bool operator >=(Unit _, Unit __) => true;
    public static bool operator <(Unit _, Unit __) => false;
    public static bool operator <=(Unit _, Unit __) => true;
    public int CompareTo(Unit unit) => 0;
    public bool Equals(Unit unit) => true;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unit;
    public override int GetHashCode() => typeof(Unit).GetHashCode();
    public override string ToString() => "()";
}