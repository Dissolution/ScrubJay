namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class ArrayExtensions
{
    extension<T>(T[]? array)
    {
        /// <summary>
        /// Is this <c>T[]</c> <see langword="null"/> or empty?
        /// </summary>
        public bool IsNullOrEmpty() => array is null || array.Length == 0;

        /// <summary>
        /// Performs a <see cref="RefItem{T}"/> on each item in this <c>T[]</c>.
        /// </summary>
        public void ForEach(RefItem<T> perItem)
        {
            if (array is null)
                return;
            for (int i = 0; i < array.Length; i++)
            {
                perItem(ref array[i]);
            }
        }

        [return: NotNullIfNotNull(nameof(array))]
        public O[]? SelectToArray<O>(Converter<T, O> itemConverter)
        {
            if (array is null)
                return null;
            return Array.ConvertAll(array, itemConverter);
        }
    }
}