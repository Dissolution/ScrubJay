//using System.Text.Json.Serialization;

using System.Collections.Frozen;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ScrubJay.Enums;

[PublicAPI]
public enum EnumPart
{
    Name = 0,
    Value,
    Attribute,
}

public enum EnumFormatAttribute
{
    None,
    Display,                    // System.ComponentModel.DataAnnotations.DisplayAttribute
    Display_Name,
    Display_Description, 
    Display_ShortName,  
    Description,                // System.ComponentModel.DescriptionAttribute
    EnumMember,                 // System.Runtime.Serialization.EnumMemberAttribute
    DataMember,                 // System.Runtime.Serialization.DataMemberAttribute
    JsonStringEnumMemberName,   // System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute
}


public static class EnumExtensions
{
    extension(Enum)
    {
        public static string Format<E>(E @enum, string format)
            where E : struct, Enum
        {
            // todo
            return Enum.Format(typeof(E), (object)@enum, format);
        }
    }
}

public static class EnumMemberExtensions
{
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public bool HasAttribute<A>()
            where A : Attribute
        {
            throw new NotImplementedException();
        }

        public bool HasAttribute<A>([NotNullWhen(true)] out A? attribute)
        {
            throw new NotImplementedException();
        }

        public string Format(EnumPart part, string? format = null)
        {
            throw new NotImplementedException();
        }
        
        public string Format(EnumFormatAttribute formatAttribute)
        {
            throw new NotImplementedException();
        }
    }
}


public partial class EnumTypeInfo
{
    
}

public partial class EnumMemberInfo
{
    protected readonly FrozenDictionary<string, string> _attributeFormats = [];
    
    
    public Attribute[] Attributes { get; }
}

public partial class EnumMemberInfo<E> : EnumMemberInfo
    where E : struct, Enum
{
   
}