#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any<T>
{
    private static readonly Lazy<Func<T, string>> _lazyToString =
        new(CreateToStringFunc, LazyThreadSafetyMode.ExecutionAndPublication);
    
    private static string FallbackToString(T _) => typeof(T).ToString();
    
    private static Func<T, string> CreateToStringFunc()
    {
        Type instanceType = typeof(T);
        
        MethodInfo? toStringMethod = FindMethod(instanceType, "ToString", typeof(string));

        if (toStringMethod is null)
        {
            // fallback to describing the type
            return FallbackToString;
        }

        // We have to emit a dynamic method as Expressions cannot handle ref structs
        var dyn = CreateDynamicMethod("ToString", typeof(string), typeof(T));
        var gen = dyn.GetILGenerator();

        EmitLoadInstance(gen, instanceType);
        EmitCallMethod(gen, instanceType, toStringMethod);
        gen.Emit(OpCodes.Ret);

        // create the function
        Func<T, string> func;
        try
        {
            func = dyn.CreateDelegate<Func<T, string>>();
        }
        catch (Exception)
        {
            // fallback
            func = FallbackToString;
        }

        return func;
    }

    public static string ToString(T? value)
    {
        if (value is null)
            return string.Empty;
        return _lazyToString.Value.Invoke(value);
    }
}

#endif