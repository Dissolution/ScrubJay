#if !NET8_0_OR_GREATER

namespace ScrubJay.Reflection.Lightweight;

public static class UnsafeAccessor
{
    public delegate ref TValue FieldRef<TInstance, TValue>(ref TInstance instance);

    public delegate void MethodAction<TInstance>(ref TInstance instance);
    public delegate void MethodAction<TInstance, in T1>(ref TInstance instance, T1 arg1);


    public static MethodAction<TInstance, T1> GetMethodInvoker<TInstance, T1>(
        string methodName,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        var method = typeof(TInstance)
            .GetMethod(methodName, bindingFlags);
        if (method is null)
            throw new InvalidOperationException();


        var dynamicMethod = DynamicMethod.CreateDynamicMethod<MethodAction<TInstance, T1>>($"{typeof(TInstance)}_{methodName}");
        var gen = dynamicMethod.GetILGenerator();
        
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Ldarg_1);
        gen.Emit(OpCodes.Constrained, typeof(TInstance));
        gen.Emit(OpCodes.Call, method);
        gen.Emit(OpCodes.Ret);

        return dynamicMethod.CreateDelegate();
    }
}

#endif