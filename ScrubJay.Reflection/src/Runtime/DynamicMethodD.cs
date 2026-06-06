using System.Globalization;

namespace ScrubJay.Reflection.Runtime;

public sealed class DynamicMethod<D> : MethodInfo
    where D : Delegate
{
    private readonly DynamicMethod _dynamicMethod;
    private readonly Type _delegateType;

    public bool InitLocals
    {
        get => _dynamicMethod.InitLocals;
        set => _dynamicMethod.InitLocals = value;
    }
    
    internal DynamicMethod(DynamicMethod dynamicMethod)
    {
        _dynamicMethod = dynamicMethod;
        _delegateType = typeof(D);
    }

    public override object[] GetCustomAttributes(bool inherit) => _dynamicMethod.GetCustomAttributes(inherit);

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _dynamicMethod.GetCustomAttributes(attributeType, inherit);

    public override bool IsDefined(Type attributeType, bool inherit) => _dynamicMethod.IsDefined(attributeType, inherit);

    public override Type? DeclaringType => _dynamicMethod.DeclaringType;

    public override string Name => _dynamicMethod.Name;

    public override Type? ReflectedType => _dynamicMethod.ReflectedType;

    public override MethodImplAttributes GetMethodImplementationFlags() => _dynamicMethod.GetMethodImplementationFlags();

    public override ParameterInfo[] GetParameters() => _dynamicMethod.GetParameters();

    public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture) => _dynamicMethod.Invoke(obj, invokeAttr, binder, parameters, culture);

    public override MethodAttributes Attributes => _dynamicMethod.Attributes;

    public override RuntimeMethodHandle MethodHandle => _dynamicMethod.MethodHandle;

    public override MethodInfo GetBaseDefinition() => _dynamicMethod.GetBaseDefinition();

    public override ICustomAttributeProvider ReturnTypeCustomAttributes => _dynamicMethod.ReturnTypeCustomAttributes;
    
    public ParameterBuilder? DefineParameter(int position, ParameterAttributes attributes, string? parameterName)
        => _dynamicMethod.DefineParameter(position, attributes, parameterName);

    public ILGenerator GetILGenerator() => _dynamicMethod.GetILGenerator();

    public D? CreateDelegate() => _dynamicMethod.CreateDelegate(typeof(D)) as D;
}