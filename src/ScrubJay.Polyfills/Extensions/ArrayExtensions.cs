namespace ScrubJay.Polyfills;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[]? array)
    {
        public void RefEach(RefAction<T>? refItem)
        {
            if (array is null || refItem is null)
                return;

            for (var i = 0; i < array.Length; i++)
            {
                refItem(ref array[i]);
            }
        }
    }
}