#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any<T>
{
    private static readonly Lazy<Func<T?, T?, int>> _lazyCompareTo =
        new(CreateCompareToFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static int FallbackCompareTo(T? left, T? right)
    {
        if (left is null)
        {
            if (right is null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        if (right is null)
        {
            return 1;
        }

        return 0;
    }
    
    private static Func<T?, T?, int> CreateCompareToFunc()
    {
        Type instanceType = typeof(T);
        
        MethodInfo? compareToMethod = FindMethod(instanceType, "CompareTo", typeof(int), typeof(T));

        if (compareToMethod is null)
        {
            // fallback to describing the type
            return FallbackCompareTo;
        }

        // We have to emit a dynamic method as Expressions cannot handle ref structs
        var dyn = CreateDynamicMethod($"CompareTo_{typeof(T)}", typeof(int), typeof(T), typeof(T));
        var gen = dyn.GetILGenerator();

        EmitLoadInstance(gen, instanceType);
        
        // load the value to compare to
        gen.Emit(OpCodes.Ldarg_1);
        
        EmitCallMethod(gen, instanceType, compareToMethod);
        gen.Emit(OpCodes.Ret);

        // create the function
        Func<T?, T?, int> func;
        try
        {
            func = dyn.CreateDelegate<Func<T?, T?, int>>();
        }
        catch (Exception)
        {
            // fallback
            func = FallbackCompareTo;
        }

        return func;
    }

    public static int CompareTo(T? value, T? other)
    {
        return _lazyCompareTo.Value.Invoke(value, other);
    }
}

#endif