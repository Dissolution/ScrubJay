namespace ScrubJay.Polyfills;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[]? array)
    {
        public void SetAll(T item)
        {
            if (array is null) return;
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = item;
            }
        }
        
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