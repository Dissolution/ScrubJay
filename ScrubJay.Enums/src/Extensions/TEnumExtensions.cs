namespace ScrubJay.Enums.Extensions;

public static class TEnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
        public static bool HasAttribute<A>(bool inherit = true)
            where A : Attribute
        {
            return Attribute.IsDefined(typeof(E), typeof(A), inherit);
        }
        
        public static bool HasFlagsAttribute() => HasAttribute<E, FlagsAttribute>();
    }
}


//public partial class EnumTypeInfo
//{
//    
//}
//
//public partial class EnumTypeInfo<E> : EnumTypeInfo
//    where E : struct, Enum
//{
//    
//}
//
//public partial class EnumTypeInfo<E> : EnumTypeInfo
//    where E : struct, Enum
//{
//    
//}