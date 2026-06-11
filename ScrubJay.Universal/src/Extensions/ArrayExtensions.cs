namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[]? array)
    {
        public bool IsNullOrEmpty() => array is null || array.Length == 0;
    }
}