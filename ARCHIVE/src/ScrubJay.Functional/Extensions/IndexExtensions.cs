namespace ScrubJay.Functional;

[PublicAPI]
public static class IndexExtensions
{
    extension(Index index)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<int> TryGetOffset(int length)
        {
            int offset = index.GetOffset(length);
            if ((uint)offset <= (uint)length)
                return Some(offset);
            return default;
        }
    }

    extension(Index? optionalIndex)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<int> TryGetOffset(int length)
        {
            if (optionalIndex.HasValue)
            {
                int offset = optionalIndex.GetValueOrDefault().GetOffset(length);
                if ((uint)offset <= (uint)length)
                    return Some(offset);
            }
            return default;
        }
    }

    extension(Option<Index> optionalIndex)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOffset(int length, out int offset)
        {
            if (optionalIndex.IsSome(out var index))
            {
                offset = index.GetOffset(length);
                return (uint)offset <= (uint)length;
            }
            offset = -1;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<int> TryGetOffset(int length)
        {
            if (optionalIndex.IsSome(out var index))
            {
                int offset = index.GetOffset(length);
                if ((uint)offset <= (uint)length)
                    return Some(offset);
            }
            return default;
        }
    }
}