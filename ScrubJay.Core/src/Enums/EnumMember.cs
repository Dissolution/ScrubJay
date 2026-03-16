//#if !(NETFRAMEWORK || NETSTANDARD)
//using System.ComponentModel.DataAnnotations;
//#endif
//using System.Reflection;
//using System.Runtime.Serialization;
//using ScrubJay.Reflection;
//
//namespace ScrubJay.Enums;
//
//public abstract class EnumMember
//{
//    internal readonly HashSet<string> _aliases;
//
//
//    public Type EnumType { get; }
//    public Attributes Attributes { get; }
//    public string Name { get; }
//    public IReadOnlyCollection<string> Aliases => _aliases;
//    public Enum Member { get; }
//    public string Rendered { get; }
//    public int Int32Value { get; }
//    public long Int64Value { get; }
//
//    protected EnumMember(Type enumType, FieldInfo memberField)
//    {
//        Debug.Assert(enumType is not null);
//        Debug.Assert(memberField is not null);
//        Debug.Assert(memberField!.IsStatic);
//        Debug.Assert(memberField.DeclaringType == enumType);
//
//        this.EnumType = enumType;
//        this.Attributes = new(Attribute.GetCustomAttributes(memberField));
//        this.Name = memberField.Name;
//        this.Member = (Enum)memberField.GetValue(null)!;
//        this.Int32Value = ((IConvertible)Member).ToInt32(null);
//        this.Int64Value = ((IConvertible)Member).ToInt64(null);
//
//        // find aliases
//        string rendered = Name;
//        HashSet<string> aliases = new(StringComparer.OrdinalIgnoreCase)
//        {
//            Name,
//        };
//
//        if (Attributes.Contains<EnumMemberAttribute>(out var enumMemberAttribute))
//        {
//            AddAlias(enumMemberAttribute.Value);
//        }
//        if (Attributes.Contains<DescriptionAttribute>(out var descriptionAttribute))
//        {
//            AddAlias(descriptionAttribute.Description);
//        }
//#if !(NETFRAMEWORK || NETSTANDARD)
//            if (Attributes.Contains<DisplayAttribute>(out var displayAttribute))
//            {
//                AddAlias(displayAttribute.Description);
//                AddAlias(displayAttribute.ShortName);
//                AddAlias(displayAttribute.Name);
//            }
//#endif
//
//        aliases.TrimExcess();
//        _aliases = aliases;
//        this.Rendered = rendered;
//        return;
//
//        void AddAlias(string? alias)
//        {
//            if (alias is not null)
//            {
//                if (aliases.Add(alias))
//                {
//                    rendered = alias;
//                }
//            }
//        }
//    }
//
//}
//
//public sealed class EnumMember<E> : EnumMember
//    where E : struct, Enum
//{
//    public new E Member { get; }
//
//    internal EnumMember(FieldInfo memberField)
//        : base(typeof(E), memberField)
//    {
//        this.Member = (E)base.Member;
//    }
//}