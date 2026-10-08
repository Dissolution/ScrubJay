using System.Globalization;

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public sealed class DynamicMethod<D> : MethodInfo
    where D : Delegate
{
    private readonly DynamicMethod _dynamicMethod;

    public override string Name => _dynamicMethod.Name;

    public override Type? DeclaringType => _dynamicMethod.DeclaringType;

    public override Type? ReflectedType => _dynamicMethod.ReflectedType;

    public override RuntimeMethodHandle MethodHandle => _dynamicMethod.MethodHandle;

    public override MethodAttributes Attributes => _dynamicMethod.Attributes;

    public override CallingConventions CallingConvention => _dynamicMethod.CallingConvention;

    public override MethodInfo GetBaseDefinition() => this;

    public override ParameterInfo[] GetParameters() => _dynamicMethod.GetParameters();

    public override MethodImplAttributes GetMethodImplementationFlags() => _dynamicMethod.GetMethodImplementationFlags();

    public override bool IsSecurityCritical => true;

    public override bool IsSecuritySafeCritical => false;

    public override bool IsSecurityTransparent => false;

    public DynamicMethod(DynamicMethod dynamicMethod)
    {
        _dynamicMethod = dynamicMethod;
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _dynamicMethod.GetCustomAttributes(attributeType, inherit);

    public override object[] GetCustomAttributes(bool inherit) => _dynamicMethod.GetCustomAttributes(inherit);

    public override bool IsDefined(Type attributeType, bool inherit) => _dynamicMethod.IsDefined(attributeType, inherit);

    public override Type ReturnType => _dynamicMethod.ReturnType;


    public override
#if NET6_0_OR_GREATER
        ParameterInfo
#else
        // ReSharper disable once ReturnTypeCanBeNotNullable
        ParameterInfo?
#endif
        ReturnParameter => _dynamicMethod.ReturnParameter;

    public override ICustomAttributeProvider ReturnTypeCustomAttributes => _dynamicMethod.ReturnTypeCustomAttributes;

    public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
    {
        return _dynamicMethod.Invoke(obj, invokeAttr, binder, parameters, culture!);
    }

    public ILGenerator GetILGenerator() => _dynamicMethod.GetILGenerator();

    public D CreateDelegate()
    {
        return _dynamicMethod.CreateDelegate<D>();
    }

    public bool TryCreateDelegate([NotNullWhen(true)] out D? @delegate)
    {
        try
        {
            @delegate = _dynamicMethod.CreateDelegate<D>();
            return true;
        }
        catch
        {
            @delegate = null;
            return false;
        }
    }
}