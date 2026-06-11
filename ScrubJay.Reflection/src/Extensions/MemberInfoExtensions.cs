namespace ScrubJay.Reflection.Extensions;

public static class MemberInfoExtensions
{
    extension(MemberInfo? member)
    {
        [NotNullIfNotNull(nameof(member))]
        public Type? ParentType
        {
            get
            {
                if (member is null)
                    return null;
                return member.DeclaringType ??
                       member.ReflectedType ??
                       member.Module.GetType();
            }
        }
        
        public bool IsStatic
        {
            get
            {
                switch (member)
                {
                    case null:
                        return false;
                    case Type type:
                        return type.IsStatic;
                    case FieldInfo @field:
                        return @field.IsStatic;
                    case MethodBase method:
                        return method.IsStatic;
                    case PropertyInfo property:
                    {
                        if (property.GetMethod is not null)
                            return property.GetMethod.IsStatic;
                        if (property.SetMethod is not null)
                            return property.SetMethod.IsStatic;
                        return property.ParentType.IsStatic;
                    }
                    case EventInfo @event:
                    {
                        if (@event.AddMethod is not null)
                            return @event.AddMethod.IsStatic;
                        if (@event.RemoveMethod is not null)
                            return @event.RemoveMethod.IsStatic;
                        if (@event.RaiseMethod is not null)
                            return @event.RaiseMethod.IsStatic;
                        return @event.ParentType.IsStatic;
                    }
                    default:
                        return false;
                }
            }
        }
        
        public Visibility Visibility
        {
            get
            {
                return member switch
                {
                    FieldInfo fieldInfo => fieldInfo.Visibility,
                    PropertyInfo propertyInfo => propertyInfo.Visibility,
                    EventInfo eventInfo => eventInfo.Visibility,
                    ConstructorInfo ctor => ctor.Visibility,
                    MethodInfo method => method.Visibility,
                    MethodBase methodBase => methodBase.Visibility,
                    Type type => type.Visibility,
                    _ => Visibility.None,
                };
            }
        }

        public Attribute[] GetAttributes()
        {
            if (member is null)
                return [];
            return Attribute.GetCustomAttributes(member, true);
        }
    }
}