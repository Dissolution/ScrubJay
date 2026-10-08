namespace ScrubJay.Text.Collections;

[PublicAPI]
public static class StackExtensions
{
    extension(stack<char> charStack)
    {
        /// <summary>
        /// Ensures that the builder is terminated with a NUL character.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NullTerminate() => charStack.Push('\0');
        
        private void AppendSlow(string s)
        {
            int pos = _position;
            if (pos > _span.Length - s.Length)
            {
                Grow(s.Length);
            }

            s
#if !NET
                .AsSpan()
#endif
                .CopyTo(_span.Slice(pos));
            _position += s.Length;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string? s)
        {
            if (s == null)
            {
                return;
            }

            int pos = _position;
            if (s.Length == 1 && (uint)pos < (uint)_span.Length) // very common case, e.g. appending strings from NumberFormatInfo like separators, percent symbols, etc.
            {
                _span[pos] = s[0];
                _position = pos + 1;
            }
            else
            {
                AppendSlow(s);
            }
        }
        
        public void Insert(int index, string? str)
        {
            if (str is null)
            {
                return;
            }

            int count = str.Length;

            if (_position > (_span.Length - count))
            {
                Grow(count);
            }

            int remaining = _position - index;
            _span.Slice(index, remaining).CopyTo(_span.Slice(index + count));
            str
#if !NET
                .AsSpan()
#endif
                .CopyTo(_span.Slice(index));
            _position += count;
        }
    }
}