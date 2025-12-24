
using System.Diagnostics;
using System.Reflection;
using ConsoleSandbox;
using ScrubJay.Functional;
using ScrubJay.Interpolated;
using ScrubJay.Testing;
using ScrubJay.Universal;



//
// var elementType = type.GetElementType();
// var elementTypeAttributes = Attribute.GetCustomAttributes(elementType!, true);
// var elementTypeNullable = elementType.Nullability;
// var elementTypeGenericTypes = elementType!.GetGenericArguments();

Debugger.Break();


return;

static class Util
{
    
    
    public static bool IsGenericArgumentNullable(Type genericType, int argumentIndex)
    {
        var attr = genericType.GetCustomAttribute<NullableAttribute>();
        if (attr?.NullableFlags != null && attr.NullableFlags.Length > argumentIndex + 1)
        {
            return attr.NullableFlags[argumentIndex + 1] == 2;
        }

        return false; // Unable to determine, assume not nullable
    }
}