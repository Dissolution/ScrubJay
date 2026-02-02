namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type type)
    {
        public bool IsDelegate => typeof(MulticastDelegate).IsAssignableFrom(type);

    }
}