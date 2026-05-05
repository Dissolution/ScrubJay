namespace ScrubJay.Enums.Extensions;

public static class TEnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
       //public static bool operator ==(E left, E right) => TEnumInstanceExtensions.Equals(left, right);
       //public static bool operator !=(E left, E right) => !TEnumInstanceExtensions.Equals(left, right);
       //public static bool operator <(E left, E right) => TEnumInstanceExtensions.CompareTo(left, right) < 0;
       //public static bool operator <=(E left, E right) => TEnumInstanceExtensions.CompareTo(left, right) <= 0;
       //public static bool operator >(E left, E right) => TEnumInstanceExtensions.CompareTo(left, right) > 0;
       //public static bool operator >=(E left, E right) => TEnumInstanceExtensions.CompareTo(left, right) >= 0;

       //public static E operator &(E left, E right) => TFlaggedEnumInstanceExtensions.And(left, right);
       //public static E operator |(E left, E right) => TFlaggedEnumInstanceExtensions.Or(left, right);
       //public static E operator ^(E left, E right) => TFlaggedEnumInstanceExtensions.Xor(left, right);

       //public static E operator ~(E @enum) => TFlaggedEnumInstanceExtensions.Negate(@enum);

        public static bool HasAttribute<A>(bool inherit = true)
            where A : Attribute
        {
            return Attribute.IsDefined(typeof(E), typeof(A), inherit);
        }

        public static bool HasFlagsAttribute() => HasAttribute<E, FlagsAttribute>();
    }
}