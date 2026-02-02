using System.Reflection;
using ScrubJay.Reflection;

namespace ScrubJay.Destructuring;

partial class Destructure
{
    extension(TextBuilder builder)
    {
        public TextBuilder DestructMember(MemberInfo? member) => member switch
        {
            null => builder,
            FieldInfo => builder.DestructMember(member, DestructureOptions.ForField),
            PropertyInfo => builder.DestructMember(member, DestructureOptions.ForProperty),
            EventInfo => builder.DestructMember(member, DestructureOptions.ForEvent),
            ConstructorInfo => builder.DestructMember(member, DestructureOptions.ForConstructor),
            MethodInfo => builder.DestructMember(member, DestructureOptions.ForMethod),
            Type => builder.DestructMember(member, DestructureOptions.ForType),
            _ => builder.DestructMember(member, DestructureOptions.All),
        };

        public TextBuilder DestructMember(MemberInfo? member, DestructureOptions options)
        {
            if (member is null)
                return builder;

            if (member is Type type)
            {
                return builder.Append(TypeName.For(type));
            }

            bool appended = false;

            if (options.HasFlags(DestructureOptions.Visibility))
            {
                var viz = member.Visibility;
                var flags = FlagsEnumExtensions.GetFlags<Visibility>(viz);
#pragma warning disable CA1308
                builder.Delimit(' ', flags, (tb, flag) => tb.Append(flag.ToString().ToLowerInvariant()));
#pragma warning restore CA1308
                appended = true;
            }
            
            if (options.HasFlags(DestructureOptions.Type))
            {
                if (appended)
                    builder.Write(' ');
                
                if (member is FieldInfo field)
                {
                    builder.DestructMember(field.FieldType, options);
                }
                else if (member is PropertyInfo propertyInfo)
                {
                    builder.DestructMember(propertyInfo.PropertyType, options);
                }
                else if (member is EventInfo @event)
                {
                    builder.DestructMember(@event.EventHandlerType, options);
                }
                else if (member is ConstructorInfo ctor)
                {
                    builder.DestructMember(ctor.ParentType, options);
                }
                else if (member is MethodInfo method)
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    if (method.ReturnParameter is null)
                    {
                        builder.DestructMember(method.ReturnType, options);
                    }
                    else
                    {
                        builder.DestructParameter(method.ReturnParameter, options);
                    }
                }
                else
                {
                    throw Ex.Arg(member);
                }
                appended = true;
            }

            if (options.HasFlags(DestructureOptions.DeclaringType))
            {
                if (member.DeclaringType is not null)
                {
                    if (appended)
                        builder.Append(' ');
                    builder.DestructMember(member.DeclaringType, DestructureOptions.Simple).Append('.');
                    appended = false;
                }
            }

            if (options.HasFlags(DestructureOptions.Name))
            {
                if (appended)
                {
                    builder.Write(' ');
                }

                builder.Write(member.Name);

                appended = true;
            }

            if (options.HasFlags(DestructureOptions.GenericTypes))
            {
                var genericTypes = member.GetGenericTypes();
                if (genericTypes.Length > 0)
                {
                    builder.Append('<')
                        .Delimit(", ", genericTypes, static (tb, gt) => tb.DestructType(gt))
                        .Write('>');
                    appended = true;
                }
            }


            if (options.HasFlags(DestructureOptions.Parameters))
            {
                if (member is MethodBase method)
                {
                    builder.Append('(')
                        .Delimit(", ", method.GetParameters(),
                            (tb, parameter) => tb.DestructParameter(parameter))
                        .Append(')');
                    appended = true;
                }
                else if (member is PropertyInfo property)
                {
                    var indexers = property.GetIndexParameters();
                    if (indexers.Length > 0)
                    {
                        builder.Append('[')
                            .Delimit(", ", indexers,
                                (tb, parameter) => tb.DestructParameter(parameter))
                            .Append(']');
                        appended = true;
                    }
                }
            }

            if (options.HasFlags(DestructureOptions.Access))
            {
                if (member is PropertyInfo property)
                {
                    if (appended)
                    {
                        builder.Write(' ');
                    }

                    builder.Write("{ ");

                    // get?
                    if (property.CanRead)
                    {
                        builder.Write("get; ");
                    }

                    if (property.SetMethod is not null) // CanWrite
                    {
                        var setMethod = property.SetMethod;

                        // Get the modifiers applied to the return parameter.
                        var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

                        // Init-only properties are marked with the IsExternalInit type.
                        bool isInit = setMethodReturnParameterModifiers.Contains(typeof(IsExternalInit));

                        if (isInit)
                        {
                            builder.Write("init; ");
                        }
                        else
                        {
                            builder.Write("set; ");
                        }
                    }

                    builder.Write('}');
                    appended = true;
                }
            }

            return builder;
        }


        public TextBuilder DestructType(Type? type, DestructureOptions options = DestructureOptions.ForType)
        {
            return builder.DestructMember(type, options);
        }

        public TextBuilder DestructType<T>(DestructureOptions options = DestructureOptions.ForType)
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            return builder.DestructMember(typeof(T), options);
        }

        public TextBuilder DestructParameter(
            ParameterInfo? parameter,
            DestructureOptions options = DestructureOptions.ForParameter)
        {
            if (parameter is null)
                return builder;

            bool appended = false;

            if (options.HasFlags(DestructureOptions.Type))
            {
                builder.DestructType(parameter.ParameterType, options);
                appended = true;
            }

            if (options.HasFlags(DestructureOptions.Name))
            {
                if (appended)
                {
                    builder.Write(' ');
                }

                builder.Write(parameter.Name);
                appended = true;
            }

            if (options.HasFlags(DestructureOptions.Parameters))
            {
                if (parameter.HasDefaultValue)
                {
                    if (appended)
                    {
                        builder.Write(' ');
                    }

                    builder.Append("= ").Destruct(parameter.DefaultValue);
                    appended = true;
                }
            }

            return builder;
        }
    }
}