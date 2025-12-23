#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any<T>
{
    private static readonly Lazy<Func<T?, object?, bool>> _lazyEquals =
        new(CreateEqualsFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static bool FallbackEquals(T? value, object? obj)
    {
        if (value is null)
            return obj is null;
        return false;
    }
    
    private static Func<T?, object?, bool> CreateEqualsFunc()
    {
        Type instanceType = typeof(T);
        
        MethodInfo? equalsMethod = FindMethod(instanceType, "Equals", typeof(bool), typeof(object));

        if (equalsMethod is null)
        {
            // fallback to describing the type
            return FallbackEquals;
        }

        // We have to emit a dynamic method as Expressions cannot handle ref structs
        var dyn = CreateDynamicMethod("Equals_Object", typeof(bool), typeof(T), typeof(object));
        var gen = dyn.GetILGenerator();

        EmitLoadInstance(gen, instanceType);
        
        // load the value to equate to
        gen.Emit(OpCodes.Ldarg_1);
        
        EmitCallMethod(gen, instanceType, equalsMethod);
        gen.Emit(OpCodes.Ret);

        // create the function
        Func<T?, object?, bool> func;
        try
        {
            func = dyn.CreateDelegate<Func<T?, object?, bool>>();
        }
        catch (Exception)
        {
            // fallback
            func = FallbackEquals;
        }

        return func;
    }

    public static bool Equals(T? value, object? obj)
    {
        return _lazyEquals.Value.Invoke(value, obj);
    }
}

#endif