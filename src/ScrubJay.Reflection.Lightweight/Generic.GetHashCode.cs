//// ReSharper disable StaticMemberInGenericType
//
//// ReSharper disable MethodOverloadWithOptionalParameter
//
//using ScrubJay.Reflection.Lightweight.Extensions;
//
//namespace ScrubJay.Reflection.Lightweight;
//
//public static partial class Any
//{
//    [OverloadResolutionPriority(100)]
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static int GetHashCode<T>(T? value)
//    {
//        if (value is null)
//            return 0;
//        return value.GetHashCode();
//    }
//
//#if NET9_0_OR_GREATER
//
//    [OverloadResolutionPriority(90)]
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static int GetHashCode<T>(in T? value)
//        where T : allows ref struct
//    {
//        return AnyGetHashCodeCache<T>.Invoke(in value);
//    }
//
//    internal delegate int AnyGetHashCode<T>(in T? value)
//        where T : allows ref struct;
//
//    internal static class AnyGetHashCodeCache<T>
//        where T : allows ref struct
//    {
//        private static readonly MethodInfo _objectGetHashCodeMethod = typeof(object).GetMethod(nameof(object.GetHashCode), BindingFlags.Public | BindingFlags.Instance)!;
//
//        public static readonly AnyGetHashCode<T> Invoke = CreateDelegate();
//
//        private static AnyGetHashCode<T> CreateDelegate()
//        {
//            var type = typeof(T);
//
//            /* ref structs cannot currently be stored in a hashtable of any sort, so GetHashCode is relatively meaningless for those instances.
//             * Span<T> and ReadOnlySpan<T> throw an NotSupportedException when their GetHashCode() is called.
//             * So we support an option: AnyGetHashCodeRefStructBehavior
//             */
//
//            if (type.IsByRefLike)
//            {
//                if (Behaviors.Any.GetHashCodeRefStructBehavior == Behaviors.AnyGetHashCodeRefStructBehavior.Throw)
//                {
//                    // We do not throw now in the construction, we throw when the method is called
//                    return static (in _) => throw new InvalidOperationException($"{typeof(T).Alias} is a `ref struct` and cannot be used in a hashtable");
//                }
//                else if (Behaviors.Any.GetHashCodeRefStructBehavior == Behaviors.AnyGetHashCodeRefStructBehavior.Passthrough)
//                {
//                    
//                }
//                else if (Behaviors.Any.GetHashCodeRefStructBehavior == Behaviors.AnyGetHashCodeRefStructBehavior.Address)
//                {
//                    return DynamicMethod.Generate<AnyGetHashCode<T>>(
//                        $"Any_{type.Alias}_GetHashCode",
//                        generator =>
//                        {
//                            generator.Emit(OpCodes.Ldarg_0);
//                            generator.Emit(OpCodes.Conv_U);
//                            generator.Emit(OpCodes.Conv_U8);
//                            generator.Emit(OpCodes.Dup);
//                            generator.Emit(OpCodes.Ldc_I4, 32);
//                            generator.Emit(OpCodes.Shr_Un);
//                            generator.Emit(OpCodes.Xor);
//                            generator.Emit(OpCodes.Conv_I4);
//                            generator.Emit(OpCodes.Ret);
//                        });
//                }
//                else
//                {
//                    Debug.Assert(Behaviors.Any.GetHashCodeRefStructBehavior == (Behaviors.AnyGetHashCodeRefStructBehavior.Address | Behaviors.AnyGetHashCodeRefStructBehavior.Passthrough));
//                    
//                    /* Roughly:*/
//                    
//                }
//            }
//            else
//            {
//                // An instance of T may be more or less specific than T, so we have to call GetType
//
//                var dynamicMethod = DynamicMethod.Create<AnyGetType<T>>($"Any_{type.Alias}_GetType");
//                var generator = dynamicMethod.GetILGenerator();
//
//                // Do we have to emit a null check?
//                if (!type.IsValueType)
//                {
//                    Label lblNotNull = generator.DefineLabel();
//                    generator.Emit(OpCodes.Ldarg_0);
//                    generator.Emit(OpCodes.Ldind_Ref);
//                    generator.Emit(OpCodes.Brtrue, lblNotNull);
//
//                    // instance is null, return typeof(T)
//                    generator.Emit(OpCodes.Ldtoken, type);
//                    generator.Emit(OpCodes.Call, _typeGetTypeFromHandleMethod);
//                    generator.Emit(OpCodes.Ret);
//
//                    // instance is not null, continue
//                    generator.MarkLabel(lblNotNull);
//                }
//
//                // call `instance.GetType() -> Type`
//                generator.Emit(OpCodes.Ldarg_0);
//                generator.Emit(OpCodes.Constrained, type);
//                generator.Emit(OpCodes.Callvirt, _objectGetTypeMethod);
//                generator.Emit(OpCodes.Ret);
//
//                return dynamicMethod.CreateDelegate();
//            }
//           
//        }
//    }
//#endif
//
//    private static int Test<T>(in T instance)
//    {
//        Type type = default!;
//
//        try
//        {
//            return instance!.GetHashCode();
//        }
//        catch
//        {
//            nuint addr = Unsafe.GetAddress<T>(in instance);
//            ulong addr64 = (ulong)addr;
//            return (int)((addr64 >> 32) ^ addr64);
//        }
//    }
//}