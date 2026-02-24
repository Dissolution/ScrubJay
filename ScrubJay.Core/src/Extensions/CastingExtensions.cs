// ReSharper disable MergeCastWithTypeCheck
// ReSharper disable ConvertNullableToShortForm

using static InlineIL.IL;

namespace ScrubJay.Extensions;

/// <summary>
/// Extensions related to type testing and casting
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast"/>
[PublicAPI]
public static class CastingExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#the-is-operator

    extension(object? obj)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Is<T>([MaybeNullWhen(false)] out T value)
        {
            /* The obvious C# code:
            // if (obj is T)
            // {
            //     value = (T)obj;
            //     return true;
            // }
            // else
            // {
            //     value = default;
            //     return false;
            // }
             * Turns into this IL:
            IL_0000: ldarg.0      // obj
            IL_0001: isinst       !!0/*T* /
            IL_0006: brfalse.s    IL_0016
            IL_0008: ldarg.1      // 'value'
            IL_0009: ldarg.0      // obj
            IL_000a: unbox.any    !!0/*T* /
            IL_000f: stobj        !!0/*T* /
            IL_0014: ldc.i4.1
            IL_0015: ret
            IL_0016: ldarg.1      // 'value'
            IL_0017: initobj      !!0/*T* /
            IL_001d: ldc.i4.0
            IL_001e: ret
             I can do it in fewer instructions:
             */
            
            Emit.Ldarg(nameof(value));
            Emit.Ldarg(nameof(obj));
            Emit.Isinst<T>();
            Emit.Stind_Ref();
            Emit.Ldarg(nameof(value));
            Emit.Ldind_Ref();
            Emit.Ldnull();
            Emit.Cgt_Un();
            Emit.Ret();
            throw Unreachable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> Is<T>()
        {
            if (obj is T)
            {
                return Option<T>.Some((T)obj);
            }

            return default;
        }

        public Result<T> As<T>()
        {
            if (obj is T)
            {
                return Result<T>.Ok((T)obj);
            }

            if (obj is null)
            {
                if (typeof(T).CanBeNull)
                    return Result<T>.Ok(default!);
                return Ex.ArgNull<object>(nameof(obj));
            }

            return Ex.Arg(obj, $"cannot be a {Type.Render<T>()} instance");
        }
    }

    extension<T>(T? value)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNotNull([NotNullWhen(true)] out T? nonNullValue)
        {
            if (value is not null)
            {
                nonNullValue = value;
                return true;
            }
            else
            {
                nonNullValue = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> IsNotNull()
        {
            if (value is not null)
                return Some<T>(value);
            return default;
        }
    }
}