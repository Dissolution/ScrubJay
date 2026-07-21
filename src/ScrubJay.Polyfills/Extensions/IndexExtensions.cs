namespace ScrubJay.Polyfills;

[PublicAPI]
public static class IndexExtensions
{
    extension(Index index)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOffset(int length, out int offset)
        {
            offset = index.GetOffset(length);
            return (uint)offset <= (uint)length;
        }
    }

    extension(Index? optionalIndex)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOffset(int length, out int offset)
        {
            if (optionalIndex.HasValue)
            {
                offset = optionalIndex.GetValueOrDefault().GetOffset(length);
                return (uint)offset <= (uint)length;
            }
            offset = -1;
            return false;
        }
    }
}