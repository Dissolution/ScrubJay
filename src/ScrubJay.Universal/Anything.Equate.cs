// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal;

public static partial class Anything
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equate<T>(T? left, T? right) => EqualityComparer<T>.Default.Equals(left, right);

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equate<T>(in T? left, in T? right)
        where T : allows ref struct
    {
        return AnythingEquateCache<T>.Invoke(in left, in right);
    }

    internal delegate bool AnythingEquate<T>(in T? left, in T? right)
        where T : allows ref struct;

    internal static class AnythingEquateCache<T>
        where T : allows ref struct
    {
        public static readonly AnythingEquate<T> Invoke = CreateDelegate();

        private static AnythingEquate<T> CreateDelegate()
        {
            var type = typeof(T);

            // Look for an Equals method on T instances (IEquatable<T>)
            
            MethodInfo? equalsMethod = Reflect.FindInstanceMethod(
                type,
                nameof(IEquatable<>.Equals),
                typeof(bool),
                [type]);

            if (equalsMethod is null)
            {
                // We don't have any specified behavior
                
                // ref structs we can use AreSame
                if (type.IsByRefLike)
                {
                    return static (in left, in right) => Unsafe.AreSame<T>(in left, in right);
                }
                
                // class types we can use ReferenceEquals
                if (!type.IsValueType)
                {
                    return static (in left, in right) => object.ReferenceEquals(UnsafeBox(left), UnsafeBox(right));
                }
                
                // no default behavior -> use AreSame?
                return static (in left, in right) => Unsafe.AreSame(in left, in right);
            }

            var dynamicMethod = Reflect.CreateDynamicMethod<AnythingEquate<T>>($"Anything_Equate_{type.Alias}");
            var generator = dynamicMethod.GetILGenerator();

            //// Do we have to emit a null check?
            //if (!type.IsValueType)
            //{
            //    Label lblNotNull = generator.DefineLabel();
            //    generator.Emit(OpCodes.Ldarg_0);
            //    generator.Emit(OpCodes.Ldind_Ref);
            //    generator.Emit(OpCodes.Brtrue, lblNotNull);
//
            //    // instance is null, object must also be null
            //    generator.Emit(OpCodes.Ldarg_1);
            //    generator.Emit(OpCodes.Ldnull);
            //    generator.Emit(OpCodes.Ceq);
            //    generator.Emit(OpCodes.Ret);
//
            //    // instance is not null, continue
            //    generator.MarkLabel(lblNotNull);
            //}
//
            //// call `instance.Equals(object) -> bool`
            //generator.Emit(OpCodes.Ldarg_0);
            //generator.Emit(OpCodes.Ldarg_1);
            //generator.Emit(OpCodes.Constrained, type);
            //generator.Emit(OpCodes.Callvirt, equalsMethod);
            //generator.Emit(OpCodes.Ret);

            throw new NotImplementedException();
            
            return dynamicMethod.CreateDelegate<AnythingEquate<T>>();
        }
    }

#endif
}