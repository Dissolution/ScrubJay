namespace ScrubJay.Reflection;

[PublicAPI]
public static class EmissionExtensions
{
    public static bool IsShortForm(this Label label)
    {
        return (uint)label.GetHashCode() <= 127U;
    }

    public static bool IsShortForm(this LocalBuilder local)
    {
        return local.LocalIndex <= byte.MaxValue;
    }

    public static bool IsShortForm(this LocalVariableInfo local)
    {
        return local.LocalIndex <= byte.MaxValue;
    }

    
    /*
    public static void SetReturnSignature(this MethodBuilder methodBuilder, ReturnSignature signature)
    {
        _ = methodBuilder.DefineParameter(0, signature.GetParameterAttributes(), null);
    }

    public static void SetParameterSignature(this MethodBuilder methodBuilder, int index, ParameterSignature signature)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index must be zero or greater");

        var parameterBuilder = methodBuilder.DefineParameter(index + 1, signature.GetParameterAttributes(), signature.Name);
        if (signature.Default.HasSome(out var @default))
        {
            parameterBuilder.SetConstant(@default);
        }
    }

    public static void SetReturnSignature(this DynamicMethod dynamicMethod, ReturnSignature signature)
    {
        _ = dynamicMethod.DefineParameter(0, signature.GetParameterAttributes(), null);
    }

    public static void SetParameterSignature(this DynamicMethod dynamicMethod, int index, ParameterSignature signature)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index must be zero or greater");

        var parameterBuilder = dynamicMethod.DefineParameter(index + 1, signature.GetParameterAttributes(), signature.Name);
        if (signature.Default.HasSome(out var @default))
        {
            parameterBuilder?.SetConstant(@default);
        }
    }
    */
}