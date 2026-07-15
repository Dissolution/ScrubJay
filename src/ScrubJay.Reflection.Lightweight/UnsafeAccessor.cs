namespace ScrubJay.Reflection.Lightweight;

public static class UnsafeAccessor
{
    public delegate ref TValue ReferenceFieldRef<in TInstance, TValue>(TInstance instance)
        where TInstance : class;

    public delegate ref TValue ValueFieldRef<TInstance, TValue>(ref TInstance instance)
        where TInstance : struct;


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

    public static ReferenceFieldRef<TInstance, TValue> GetReferenceFieldRef<TInstance, TValue>(
        string fieldName,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        where TInstance : class
    {
        var instanceType = typeof(TInstance);
        if (instanceType.IsStatic)
            throw new InvalidOperationException();
        
        var field = typeof(TInstance).GetField(fieldName, bindingFlags);
        if (field is null || !field.FieldType.IsAssignableTo(typeof(TValue)))
            throw new InvalidOperationException();

        var del = Runtime.TryGenerateDelegate<ReferenceFieldRef<TInstance, TValue>>(
            $"access_{TypeName.For(instanceType)}_instance_field_ref_{field.Name}",
            owner: instanceType,
            gen =>
            {
                gen.Emit(OpCodes.Ldarg_0);
                if (!instanceType.IsValueType)
                {
                    gen.Emit(OpCodes.Castclass, instanceType);
                }
                gen.Emit(OpCodes.Ldflda, field);
                gen.Emit(OpCodes.Ret);
            });
        
        if (del is null)
        {
            throw new InvalidOperationException();
        }

        return del;
    }
    
//    public static ValueFieldRef<TInstance, TValue> GetValueFieldRef<TInstance, TValue>(
//        string fieldName,
//        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
//        where TInstance : struct
//    {
//        var field = typeof(TInstance)
//            .GetField(fieldName, bindingFlags);
//        if (field is null)
//            throw new InvalidOperationException();
//
//        return DynamicMethod.GenerateDelegate<ValueFieldRef<TInstance, TValue>>(
//            $"ref_{typeof(TInstance)}_{field}",
//            gen =>
//            {
//                gen.Emit(OpCodes.Ldarg_0);
//                gen.Emit(OpCodes.Ldflda, field);
//                gen.Emit(OpCodes.Ret);
//            });
//    }
}