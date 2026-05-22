#pragma warning disable CA1721

namespace ScrubJay.Reflection;

[PublicAPI]
public class OverridableParameterInfo : ParameterInfo
{
    private IList<CustomAttributeData> _customAttributeData = [];
    
    public new ParameterAttributes Attributes
    {
        get => AttrsImpl;
        set => AttrsImpl = value;
    }

    public new string? Name
    {
        get => NameImpl;
        set => NameImpl = value;
    }

    public new int Position
    {
        get => PositionImpl;
        set => PositionImpl = value;
    }

    [AllowNull]
    public new Type ParameterType
    {
        get => ClassImpl ?? typeof(void);
        set => ClassImpl = value ?? typeof(void);
    }

    public new IEnumerable<CustomAttributeData> CustomAttributes
    {
        get => base.CustomAttributes;
        set => _customAttributeData = value.ToList();
    }

    public Option<object?> Default { get; set; } = Option<object?>.None;

    public sealed override bool HasDefaultValue => Default.IsSome();

    public sealed override object? DefaultValue => Default.SomeOr(DBNull.Value);

    public sealed override object? RawDefaultValue => Default.SomeOr(DBNull.Value);
    
    public OverridableParameterInfo()
    {
        
    }

    public OverridableParameterInfo(ParameterInfo parameter)
    {
        AttrsImpl = parameter.Attributes;
        ClassImpl = parameter.ParameterType;
        PositionImpl = parameter.Position;
        NameImpl = parameter.Name;
        Default = parameter.HasDefaultValue ? Some<object?>(parameter.DefaultValue) : None;
        CustomAttributes = parameter.CustomAttributes;
    }

    public sealed override object[] GetCustomAttributes(bool inherit)
    {
        var attributes = Attribute.GetCustomAttributes(this, inherit);
        return Array.ConvertAll(attributes, static attr => (object)attr);
    }

    public sealed override object[] GetCustomAttributes([NotNull] Type attributeType, bool inherit)
    {
        var attributes = Attribute.GetCustomAttributes(this, attributeType, inherit);
        return Array.ConvertAll(attributes, static attr => (object)attr);
    }

    public sealed override IList<CustomAttributeData> GetCustomAttributesData()
    {
        return _customAttributeData;
    }
}