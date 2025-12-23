#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any<T>
{
    private static readonly Lazy<Func<T?, T?, bool>> _lazyEqualsT =
        new(CreateEqualsTFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static bool FallbackEqualsT(T? left, T? right)
    {
        if (left is null)
            return right is null;
        if (right is null)
            return false;
        return false;
    }
    
    private static Func<T?, T?, bool> CreateEqualsTFunc()
    {
        Type instanceType = typeof(T);
        
        MethodInfo? equalsMethod = FindMethod(instanceType, "Equals", typeof(bool), typeof(T));

        if (equalsMethod is null)
        {
            // fallback to describing the type
            return FallbackEqualsT;
        }

        // We have to emit a dynamic method as Expressions cannot handle ref structs
        var dyn = CreateDynamicMethod($"Equals_{typeof(T)}", typeof(bool), typeof(T), typeof(T));
        var gen = dyn.GetILGenerator();

        EmitLoadInstance(gen, instanceType);
        
        // load the value to equate to
        gen.Emit(OpCodes.Ldarg_1);
        
        EmitCallMethod(gen, instanceType, equalsMethod);
        gen.Emit(OpCodes.Ret);

        // create the function
        Func<T?, T?, bool> func;
        try
        {
            func = dyn.CreateDelegate<Func<T?, T?, bool>>();
        }
        catch (Exception)
        {
            // fallback
            func = FallbackEqualsT;
        }

        return func;
    }

    public static bool Equals(T? value, T? other)
    {
        return _lazyEqualsT.Value.Invoke(value, other);
    }
}

#endif