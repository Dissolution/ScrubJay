using static InlineIL.IL;

namespace ScrubJay.Enums.Extensions;

public static class TFlaggedEnumInstanceExtensions
{
    // readonly extensions
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public E And(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.And();
            return Return<E>();
        }

        public E Or(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.Or();
            return Return<E>();
        }

        public E Xor(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.Xor();
            return Return<E>();
        }

        /// <summary>
        /// ~
        /// </summary>
        /// <returns></returns>
        public E Negate()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Neg();
            return Return<E>();
        }
        
        
        public bool HasFlag(E flag)
        {
            throw new NotImplementedException();
        }

        public EnumEnumerable<E> AsEnumerable()
        {
            return new(@enum);
        }

        public EnumEnumerator<E> GetEnumerator()
        {
            return new(@enum);
        }

        public bool All(E flag)
        {
            throw new NotImplementedException();
        }

        public bool All(params ReadOnlySpan<E> flags)
        {
            throw new NotImplementedException();
        }

        public bool All(IEnumerable<E>? flags)
        {
            throw new NotImplementedException();
        }
        
        public bool Any(E flag)
        {
            throw new NotImplementedException();
        }

        public bool Any(params ReadOnlySpan<E> flags)
        {
            throw new NotImplementedException();
        }

        public bool Any(IEnumerable<E>? flags)
        {
            throw new NotImplementedException();
        }
    }
}