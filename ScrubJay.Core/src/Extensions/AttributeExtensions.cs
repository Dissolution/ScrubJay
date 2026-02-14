namespace ScrubJay.Extensions;

[PublicAPI]
public static class AttributeExtensions
{
    extension(Attribute[]? attributes)
    {
        /// <summary>
        /// Does this <see cref="Attribute"/> array contain an <typeparamref name="A"/>?
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <returns></returns>
        public bool Contains<A>()
            where A : Attribute
        {
            if (attributes is not null)
            {
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is A)
                        return true;
                }
            }

            return false;
        }

        public bool TryGet<A>([NotNullWhen(true)] out A? attr)
            where A : Attribute
        {
            if (attributes is not null)
            {
                foreach (Attribute attribute in attributes)
                {
                    if (attribute.Is<A>(out attr))
                        return true;
                }
            }

            attr = null;
            return false;
        }
    }
}