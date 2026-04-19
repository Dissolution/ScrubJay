//namespace ScrubJay.Text.Building;
//
//#region AppendMany
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public TextBuilder AppendMany(params text characters) => Append(characters);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public TextBuilder AppendMany(char[]? characters) => Append(new text(characters));
//
//    public TextBuilder AppendMany(IEnumerable<char>? characters)
//    {
//        if (characters is ICollection<char> collection)
//        {
//#if !NETFRAMEWORK && !NETSTANDARD
//            if (characters is List<char> list)
//            {
//                var listSpan = CollectionsMarshal.AsSpan(list);
//                return Append(listSpan);
//            }
//#endif
//
//            int count = collection.Count;
//            if (count + _position > Capacity)
//                GrowBy(count);
//            collection.CopyTo(_chars, _position);
//            _position += count;
//        }
//        else if (characters is not null)
//        {
//            foreach (char ch in characters)
//            {
//                Write(ch);
//            }
//        }
//
//        return this;
//    }
//
//    public TextBuilder AppendMany(IEnumerable<string?>? strings)
//    {
//        if (strings is not null)
//        {
//            foreach (string? str in strings)
//            {
//                Write(str);
//            }
//        }
//
//        return this;
//    }
//
//    public TextBuilder AppendMany<T>(IEnumerable<T>? values)
//    {
//        if (values is not null)
//        {
//            foreach (var value in values)
//            {
//                Append<T>(value);
//            }
//        }
//
//        return this;
//    }
//
//#if NET9_0_OR_GREATER
//    // ReSharper disable once MethodOverloadWithOptionalParameter
//    public TextBuilder AppendMany<T>(IEnumerable<T>? values, TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        if (values is not null)
//        {
//            foreach (var value in values)
//            {
//                Append<T>(value, _);
//            }
//        }
//
//        return this;
//    }
//#endif
//
//#endregion