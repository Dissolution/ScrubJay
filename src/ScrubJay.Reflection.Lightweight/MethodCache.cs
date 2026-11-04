namespace ScrubJay.Reflection.Lightweight;

internal static class MethodCache
{
    public static readonly MethodInfo Object_Equals_Object = typeof(object)
        .GetMethod(nameof(object.Equals), BindingFlags.Public | BindingFlags.Instance)!;
}