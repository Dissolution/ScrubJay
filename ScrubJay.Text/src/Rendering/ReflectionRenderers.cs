using System.Reflection;

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class ReflectionRenderers
{
    internal static void WriteNameAndGenericTypes(
        TextBuilder builder,
        string? memberName,
        Type[]? genericTypes)
    {
        if (memberName is not null)
        {
            int i = memberName.LastIndexOf('`');
            if (i >= 0)
            {
                builder.Write(memberName.AsSpan(0, i));
            }
            else
            {
                builder.Write(memberName);
            }
        }

        //string sep = type.IsGenericTypeDefinition ? "," : ", ";
        string sep = ", ";

        if (!genericTypes.IsNullOrEmpty())
        {
            builder
                .Append('<')
                .Delimit(sep, genericTypes, TB.Render)
                .Append('>');
        }
    }

    [RenderToMethod]
    public static void RenderMemberTo(MemberInfo member, TextBuilder builder)
    {
        switch (member)
        {
            case EventInfo eventInfo:
                break;
            case FieldInfo fieldInfo:
                break;
            case MethodBase method:
            {
                RenderMethodTo(method, builder);
                return;
            }
            case PropertyInfo propertyInfo:
                break;
            case Type type:
            {
                TypeRenderer.RenderTypeTo(type, builder);
                return;
            }
            default:
                break;
        }

        // nothing
        Debugger.Break();
        return;
    }

    [RenderToMethod]
    public static void RenderParameterTo(ParameterInfo parameter, TextBuilder builder)
    {
        builder.Render(parameter.ParameterType)
            .Append(' ')
            .IfNotEmpty(parameter.Name, TB.Append, TB.Append($"p{parameter.Position}"))
            .If(parameter.HasDefaultValue, tb => tb.Append(" = ").Render(parameter.DefaultValue));
    }

    [RenderToMethod]
    public static void RenderMethodTo(MethodBase method, TextBuilder builder)
    {
        // declarer
        builder.Render(method.ParentType).Append('.');

        // name
        string name = method.Name;
        if (method.IsGenericMethod)
        {
            int i = name.IndexOf('`');
            if (i >= 0)
            {
                builder.Append(name.AsSpan(0, i));
            }
            else
            {
                builder.Append(name);
            }
        }
        else
        {
            builder.Append(name);
        }

        // generic types
        var genericTypes = method.GetGenericArguments();
        if (genericTypes.Length > 0)
        {
            builder.Append('<')
                .Delimit(", ", genericTypes, TB.Render)
                .Append('>');
        }

        // parameters
        builder.Append('(')
            .Delimit(", ", method.GetParameters(), TB.Render)
            .Append(')');

        // return type
        builder.Append(" -> ");
        if (method is MethodInfo mi)
        {
            builder.Render(mi.ReturnType);
        }
        else
        {
            Debug.Assert(method is ConstructorInfo);
            builder.Render(method.ParentType);
        }

        // generic type constraints
        foreach (var genericType in genericTypes)
        {
            if (!genericType.IsGenericParameter)
                continue;
            var constraints = genericType.GetGenericParameterConstraints();
            if (constraints.Length == 0)
                continue;
            builder.Append(" where ")
                .Render(genericType)
                .Append(" : ")
                .Delimit(", ", constraints, TB.Render);
        }
    }
}