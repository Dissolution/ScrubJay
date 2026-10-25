namespace ScrubJay.Reflection.Lightweight.Extensions;

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