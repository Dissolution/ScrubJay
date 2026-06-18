// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    public static bool Equate<T>(in T? instance, in T? other)
        where T : IEquatable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is not null)
        {
            return instance.Equals(other!);
        }

        if (other is not null)
        {
            return other.Equals(instance!);
        }

        return true;
    }

    public static bool Equate<T>(in T? instance, in T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is not null)
        {
            return EqualsCache<T>.Equate(in instance, in other);
        }
        else if (other is not null)
        {
            return EqualsCache<T>.Equate(in other, in instance);
        }
        else
        {
            return true;
        }
    }
    
    private static class EqualsCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyEquals<T> Equate;
        
        static EqualsCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Func<T,T,bool>>(nameof(IEquatable<>.Equals))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyEquals<T>>(
                $"Any_{instanceType}_Equals",
                gen => gen
                    .Ldarg(0)
                    .Ldarg(1)
                    .Ldobj(instanceType)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(), out Equate!))
            {
                return;
            }

            Equate = FallbackEquals;
        }

        private static bool FallbackEquals(in T? instance, in T? other)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ceq();
            return Return<bool>();
        }
    }
}