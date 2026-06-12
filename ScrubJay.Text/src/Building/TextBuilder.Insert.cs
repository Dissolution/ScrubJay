// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public bool TryInsert(int index, char ch)
    {
        if ((uint)index <= (uint)_position)
        {
            MaybeGrowBy(1);
            TextHelper.Unsafe.ShiftItemsRight(_chars, _position, index, 1);
            _chars[index] = ch;
            _position++;
            return true;
        }
        return false;
    }

    public bool TryInsert(Index index, char ch)
    {
        if (index.TryGetOffset(_position, out int offset))
        {
            MaybeGrowBy(1);
            TextHelper.Unsafe.ShiftItemsRight(_chars, _position, offset, 1);
            _chars[index] = ch;
            _position++;
            return true;
        }
        return false;
    }

    public bool TryInsert(int index, scoped text text)
    {
        if ((uint)index <= (uint)_position)
        {
            int textLen = text.Length;
            MaybeGrowBy(textLen);
            TextHelper.Unsafe.ShiftItemsRight(_chars, _position, index, textLen);
            TextHelper.Unsafe.CopyTo(text, ref _chars[index], textLen);
            _position += textLen;
            return true;
        }
        return false;
    }

    public bool TryInsert(Index index, scoped text text)
    {
        if (index.TryGetOffset(_position, out int offset))
        {
            int textLen = text.Length;
            MaybeGrowBy(textLen);
            TextHelper.Unsafe.ShiftItemsRight(_chars, _position, offset, textLen);
            TextHelper.Unsafe.CopyTo(text, ref _chars[offset], textLen);
            _position += textLen;
            return true;
        }
        return false;
    }

    public bool TryInsert<T>(int index, T? value, Action<TextBuilder, T?>? buildValue)
    {
        if (buildValue is not null)
        {
            if ((uint)index <= (uint)_position)
            {
                // we have to build the value to know what to insert
                int start = _position;
                buildValue(this, value);
                int length = _position - start;
                if (length == 0)
                    return true;

                TextHelper.Unsafe.ShiftItemsRight(_chars, _position, index, length);
                TextHelper.Unsafe.CopyTo(ref _chars[start + length], ref _chars[index], length);
                _position -= length;
                return true;
            }
            return false;
        }
        return TryInsert<T>(index, value, TB.Append);
    }

    public bool TryInsert<T>(int index, T? value)
        => TryInsert<T>(index, value, TB.Append);


#if NET9_0_OR_GREATER
    public bool TryInsert<T>(int index, T? value, Action<TextBuilder, T?>? buildValue,
        TypeConstraints.AllowsRefStruct<T> _ = default)
    where T : allows ref struct
    {
        buildValue ??= TB.Append<T>(_);

        if ((uint)index <= (uint)_position)
        {
            // we have to build the value to know what to insert
            int start = _position;
            buildValue(this, value);
            int length = _position - start;
            if (length == 0)
                return true;

            TextHelper.Unsafe.ShiftItemsRight(_chars, _position, index, length);
            TextHelper.Unsafe.CopyTo(ref _chars[start + length], ref _chars[index], length);
            _position -= length;
            return true;
        }
        return false;
    }
#endif
}