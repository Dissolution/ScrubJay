#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any<T>
{
    private static readonly Lazy<Func<T, int>> _lazyGetHashCode =
        new(CreateGetHashCodeFunc, LazyThreadSafetyMode.ExecutionAndPublication);
    
    private static int FallbackGetHashCode(T _) => typeof(T).GetHashCode();
    
    private static Func<T, int> CreateGetHashCodeFunc()
    {
        Type instanceType = typeof(T);
        
        MethodInfo? getHashCodeMethod = FindMethod(instanceType, "GetHashCode", typeof(int));

        if (getHashCodeMethod is null)
        {
            // fallback to describing the type
            return FallbackGetHashCode;
        }

        // We have to emit a dynamic method as Expressions cannot handle ref structs
        var dyn = CreateDynamicMethod("GetHashCode", typeof(int), typeof(T));
        var gen = dyn.GetILGenerator();

        EmitLoadInstance(gen, instanceType);
        EmitCallMethod(gen, instanceType, getHashCodeMethod);
        gen.Emit(OpCodes.Ret);

        // create the function
        Func<T, int> func;
        try
        {
            func = dyn.CreateDelegate<Func<T, int>>();
        }
        catch (Exception)
        {
            // fallback
            func = FallbackGetHashCode;
        }

        return func;
    }

    public static int GetHashCode(T? value)
    {
        if (value is null)
            return 0;
        return _lazyGetHashCode.Value.Invoke(value);
    }
}

#endif