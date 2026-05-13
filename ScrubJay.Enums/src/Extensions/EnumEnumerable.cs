//namespace ScrubJay.Enums.Extensions;
//
//public struct EnumEnumerable<E> : IEnumerable<E>
//    where E : struct, Enum
//{
//    public EnumEnumerable(E @enum)
//    {
//    }
//
//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//
//    IEnumerator<E> IEnumerable<E>.GetEnumerator() => GetEnumerator();
//
//    public EnumEnumerator<E> GetEnumerator()
//    {
//        throw new NotImplementedException();
//    }
//}
//
//public readonly struct Flags<E> :
//    IReadOnlyCollection<E>, IEnumerable<E>,
//    IEquatable<E>, IComparable<E>,
//#if NET7_0_OR_GREATER
//    IEqualityOperators<Flags<E>, E, bool>,
//    IComparisonOperators<Flags<E>, E, bool>,
//#endif
//#if NET6_0_OR_GREATER
//    IReadOnlySet<E>,
//#endif
//    IFormattable
//    where E : struct, Enum
//{
//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//
//    public IEnumerator<E> GetEnumerator() => throw new NotImplementedException();
//
//    public int Count { get; }
//    public bool Contains(E item) => throw new NotImplementedException();
//
//    public bool IsProperSubsetOf(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool IsProperSupersetOf(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool IsSubsetOf(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool IsSupersetOf(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool Overlaps(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool SetEquals(IEnumerable<E> other) => throw new NotImplementedException();
//
//    public bool Equals(E other) => throw new NotImplementedException();
//
//    public int CompareTo(E other) => throw new NotImplementedException();
//
//    public static bool operator ==(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public static bool operator !=(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public static bool operator >(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public static bool operator >=(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public static bool operator <(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public static bool operator <=(Flags<E> left, E right) => throw new NotImplementedException();
//
//    public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
//
//    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
//
//    public override int GetHashCode() => base.GetHashCode();
//
//    public override string ToString() => base.ToString()!;
//}