namespace ScrubJay.Universal.Reflection;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type? type)
    {
#if NETFRAMEWORK || NETSTANDARD2_0
        public bool IsByRefLike
        {
            get
            {
                if (type is null)
                    return false;
                return Attribute
                    .GetCustomAttributes(type, false)
                    .Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.IsByRefLikeAttribute");
            }
        }
#endif

        public bool IsStatic => type is { IsAbstract: true, IsSealed: true };
    }
}