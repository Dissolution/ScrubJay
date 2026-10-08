namespace ScrubJay.Reflection.Lightweight;

public static class TypeAttributesExtensions
{
    extension(TypeAttributes)
    {
        public static TypeAttributes Static
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => TypeAttributes.Abstract | TypeAttributes.Sealed;
        }
    }
}