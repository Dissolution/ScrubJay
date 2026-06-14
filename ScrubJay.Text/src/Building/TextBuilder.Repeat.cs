// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    #region Repeat Write
    public TextBuilder Repeat(int count, char ch)
    {
        if (count > 0)
        {
            MaybeGrowBy(count);
            _chars.AsSpan(_position, count).Fill(ch);
            _position += count;
        }

        return this;
    }

    public TextBuilder Repeat(int count, scoped text text)
    {
        int len = text.Length;
        if (count > 0 && len > 0)
        {
            int pos = _position;
            int newPos = pos + (len * count);
            if (newPos > Capacity)
            {
                GrowTo(newPos);
            }

            var span = _chars.AsSpan();
            for (int i = 0; i < count; i++, pos += len)
            {
                TextHelper.Unsafe.CopyTo(text, ref span[pos], len);
            }

            Debug.Assert(pos == newPos);
            _position = newPos;
        }

        return this;
    }


    public TextBuilder Repeat(int count, string? str) => Repeat(count, str.AsSpan());

    public TextBuilder Repeat<T>(int count, T? value)
    {
        if (count > 0 && value is not null)
        {
            int start = _position;
            Write<T>(value);
            var written = _chars.AsSpan(start, _position - start);
            int writtenLen = written.Length;
            int newPos = writtenLen * (count - 1);
            MaybeGrowBy(newPos);
            ref char d = ref _chars[_position];
            for (var i = 1; i < count; i++)
            {
                TextHelper.Unsafe.CopyTo(written, ref d, writtenLen);
                d = ref Unsafe.Add<char>(ref d, writtenLen);
            }
            _position = newPos;
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Repeat<T>(int count, T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (count > 0 && value is not null)
        {
            int start = _position;
            Write<T>(value, _);
            var written = _chars.AsSpan(start, _position - start);
            int writtenLen = written.Length;
            int newPos = writtenLen * (count - 1);
            MaybeGrowBy(newPos);
            ref char d = ref _chars[_position];
            for (var i = 1; i < count; i++)
            {
                TextHelper.Unsafe.CopyTo(written, ref d, writtenLen);
                d = ref Unsafe.Add<char>(ref d, writtenLen);
            }
            _position = newPos;
        }

        return this;
    }
#endif
    #endregion

    #region Repeat Action
    public TextBuilder Repeat(int count, Action<TextBuilder>? build)
    {
        if (build is not null)
        {
            for (int i = 0; i < count; i++)
            {
                build(this);
            }
        }

        return this;
    }

    public TextBuilder Repeat<T>(int count, T value, Action<TextBuilder, T>? buildItem)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (buildItem is not null)
        {
            for (int i = 0; i < count; i++)
            {
                buildItem(this, value);
            }
            return this;
        }
        return Repeat<T>(count, value);
    }
    #endregion

}