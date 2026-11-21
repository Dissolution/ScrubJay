namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class IndexExtensions
{
    extension(Index index)
    {
        public bool TryGetOffset(int length, out int offset)
        {
            offset = index.GetOffset(length);
            if ((uint)offset < (uint)length)
                return true;

            offset = -1;
            return false;
        }

        public Option<int> TryGetOffset(int length)
        {
            var offset = index.GetOffset(length);
            if ((uint)offset < (uint)length)
                return offset;

            return default;
        }
    }
}