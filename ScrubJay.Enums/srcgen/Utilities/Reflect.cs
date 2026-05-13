using System.Linq.Expressions;
using System.Reflection;

namespace ScrubJay.Enums.SourceGen.Utilities;

internal static class Reflect
{
    private static MemberInfo? GetMemberInfo(Expression? expression)
    {
        if (expression is LambdaExpression lambdaExpression)
        {
            return GetMemberInfo(lambdaExpression.Body);
        }
        if (expression is MemberExpression memberExpression)
        {
            return memberExpression.Member;
        }
        throw new NotImplementedException();
        //return null;
    }

    private static FieldInfo? GetBackingField(PropertyInfo? propertyInfo)
    {
        if (propertyInfo is not null)
        {
            var backingFieldName = $"<{propertyInfo.Name}>k__BackingField";
            var backingField = propertyInfo.DeclaringType!.GetField(backingFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (backingField is not null)
            {
                return backingField;
            }
        }
       
        return null;
    }
    
    public static void SetMember<I, F>(I instance, Expression<Func<I,F>> selectMemberExpression, F value)
    {
        var member = GetMemberInfo(selectMemberExpression);
        if (member is PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, value);
                return;
            }
            
            member = GetBackingField(propertyInfo);
        }

        if (member is FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(instance, value);
            return;
        }

        throw new InvalidOperationException();
    }
}