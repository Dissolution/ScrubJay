using System.Reflection;
using ScrubJay.Reflection;

namespace ScrubJay.Extensions;

[PublicAPI]
public static class ReflectionExtensions
{
    extension(MemberInfo? member)
    {
        public bool IsGeneric
        {
            get
            {
                return member switch
                {
                    Type type => type.IsGenericType,
                    MethodBase methodBase => methodBase.IsGenericMethod,
                    PropertyInfo propertyInfo =>
                        (propertyInfo.GetMethod ?? propertyInfo.SetMethod)?.IsGenericMethod == true,
                    EventInfo eventInfo =>
                        (eventInfo.AddMethod ?? eventInfo.RemoveMethod ?? eventInfo.RaiseMethod)?.IsGenericMethod ==
                        true,
                    _ => false,
                };
            }
        }
        
        public Type? OwningType
        {
            get
            {
                if (member is null)
                    return null;
                return member.DeclaringType ?? member.ReflectedType ?? member.Module.GetType();
            }
        }

        public Visibility Visibility
        {
            get
            {
                switch (member)
                {
                    case null:
                        return Visibility.None;
                    case EventInfo eventInfo:
                    {
                        Visibility viz = Visibility.None;
                        viz.AddFlag(eventInfo.AddMethod.Visibility);
                        viz.AddFlag(eventInfo.RemoveMethod.Visibility);
                        viz.AddFlag(eventInfo.RaiseMethod.Visibility);
                        return viz;
                    }
                    case FieldInfo @field:
                    {
                        Visibility viz = Visibility.None;
                        if (@field.IsStatic)
                        {
                            viz.AddFlag(Visibility.Static);
                        }
                        else
                        {
                            viz.AddFlag(Visibility.Instance);
                        }

                        if (@field.IsPrivate)
                        {
                            viz.AddFlag(Visibility.Private);
                        }

                        if (@field.IsFamily || @field.IsFamilyOrAssembly || @field.IsFamilyAndAssembly)
                        {
                            viz.AddFlag(Visibility.Protected);
                        }

                        if (@field.IsAssembly || @field.IsFamilyOrAssembly || @field.IsFamilyAndAssembly)
                        {
                            viz.AddFlag(Visibility.Internal);
                        }

                        if (@field.IsPublic)
                        {
                            viz.AddFlag(Visibility.Public);
                        }

                        return viz;
                    }
                    case MethodBase method:
                    {
                        Visibility viz = Visibility.None;
                        if (method.IsStatic)
                        {
                            viz.AddFlag(Visibility.Static);
                        }
                        else
                        {
                            viz.AddFlag(Visibility.Instance);
                        }

                        if (method.IsPrivate)
                        {
                            viz.AddFlag(Visibility.Private);
                        }

                        if (method.IsFamily || method.IsFamilyOrAssembly || method.IsFamilyAndAssembly)
                        {
                            viz.AddFlag(Visibility.Protected);
                        }

                        if (method.IsAssembly || method.IsFamilyOrAssembly || method.IsFamilyAndAssembly)
                        {
                            viz.AddFlag(Visibility.Internal);
                        }

                        if (method.IsPublic)
                        {
                            viz.AddFlag(Visibility.Public);
                        }

                        return viz;
                    }
                    case PropertyInfo propertyInfo:
                    {
                        Visibility viz = Visibility.None;
                        viz.AddFlag(propertyInfo.GetMethod.Visibility);
                        viz.AddFlag(propertyInfo.SetMethod.Visibility);
                        return viz;
                    }
                    case Type type:
                    {
                        Visibility viz = Visibility.None;
                        if (type.IsStatic)
                        {
                            viz.AddFlag(Visibility.Static);
                        }
                        else
                        {
                            viz.AddFlag(Visibility.Instance);
                        }

                        if (type.IsPublic)
                        {
                            viz.AddFlag(Visibility.Public);
                        }

                        if (type.IsNotPublic)
                        {
                            viz.AddFlag(Visibility.NonPublic);
                        }

                        if (type.IsNested)
                        {
                            viz.AddFlag(type.OwningType.Visibility);

                            if (type.IsNestedPrivate)
                            {
                                viz.AddFlag(Visibility.Private);
                            }

                            if (type.IsNestedFamily || type.IsNestedFamORAssem || type.IsNestedFamANDAssem)
                            {
                                viz.AddFlag(Visibility.Protected);
                            }

                            if (type.IsNestedAssembly || type.IsNestedFamORAssem || type.IsNestedFamANDAssem)
                            {
                                viz.AddFlag(Visibility.Internal);
                            }

                            if (type.IsNestedPublic)
                            {
                                viz.AddFlag(Visibility.Public);
                            }
                        }

                        return viz;
                    }
                    default:
                        return Visibility.None;
                }
            }
        }

        public Type[] GetGenericTypes()
        {
            return member switch
            {
                Type type => type.GetGenericArguments(),
                MethodBase methodBase => methodBase.GetGenericArguments(),
                PropertyInfo propertyInfo =>
                    (propertyInfo.GetMethod ?? propertyInfo.SetMethod)?.GetGenericArguments() ?? [],
                EventInfo eventInfo =>
                    (eventInfo.AddMethod ?? eventInfo.RemoveMethod ?? eventInfo.RaiseMethod)?.GetGenericArguments() ??
                    [],
                _ => [],
            };
        }

        public Attribute[] GetAttributes(bool inherit = true)
        {
            if (member is null)
                return [];
            return Attribute.GetCustomAttributes(member, inherit);
        }
    }
}