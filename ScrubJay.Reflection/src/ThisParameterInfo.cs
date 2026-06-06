using ScrubJay.Errors.Validation;
using ScrubJay.Reflection.Validation;

namespace ScrubJay.Reflection;

/// <summary>
/// An instance of the <c>this</c> <see cref="ParameterInfo"/> for an instance <see cref="MethodBase"/>
/// </summary>
[PublicAPI]
public sealed class ThisParameterInfo : ParameterInfo
{
    public override bool HasDefaultValue => false;

    public override object? DefaultValue => DBNull.Value;

    public override object? RawDefaultValue => DBNull.Value;
    
    public ThisParameterInfo(MethodBase method)
    {
        Demand.NotStatic(method);
        MemberImpl = method;
        ClassImpl = method.DeclaringType.ThrowIfNull();
        NameImpl = "this";
        PositionImpl = 0;
    }

    public override object[] GetCustomAttributes(bool inherit) => [];
    
    public override object[] GetCustomAttributes(Type? attributeType, bool inherit) => [];
    
    public override IList<CustomAttributeData> GetCustomAttributesData() => [];
}