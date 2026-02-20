//#pragma warning disable MA0074
//
//namespace ScrubJay.Text;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Auto)]
//public readonly ref struct txt
//    // :
//    // IEqualityOperators<txt, txt, bool>,
//    // IComparisonOperators<txt, txt, bool>,
//    // IEquatable<txt>,
//    // IComparable<txt>
//{
//    public static implicit operator txt(in char ch) => new txt(in ch);
//    public static implicit operator txt(ReadOnlySpan<char> characters) => new txt(characters);
//    public static implicit operator txt(string? str) => new txt(str);
//
//    public static implicit operator ReadOnlySpan<char>(txt text) => text.AsSpan();
//
//    public static bool operator ==(txt left, txt right) => Equate(left, right);
//    public static bool operator !=(txt left, txt right) => !Equate(left, right);
//    public static bool operator <(txt left, txt right) => Compare(left, right) < 0;
//    public static bool operator <=(txt left, txt right) => Compare(left, right) <= 0;
//    public static bool operator >=(txt left, txt right) => Compare(left, right) >= 0;
//    public static bool operator >(txt left, txt right) => Compare(left, right) > 0;
//
//
//
//    public static bool Equate(txt left, txt right)
//    {
//        throw Ex.NotImplemented();
//    }
//    
//    public static bool Equate(txt left, txt right, StringComparison comparison)
//    {
//        throw Ex.NotImplemented();
//    }
//
//    public static int Compare(txt left, txt right)
//    {
//        throw Ex.NotImplemented();
//    }
//    
//    public static int Compare(txt left, txt right, StringComparison comparison)
//    {
//        throw Ex.NotImplemented();
//    }
//
//
//    private readonly ref readonly char _firstChar;
//    private readonly int _length;
//
//    public ref readonly char this[int index]
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get
//        {
//            if ((uint)index >= (uint)_length)
//                throw new IndexOutOfRangeException();
//            return ref Notsafe.OffsetReadOnlyRef(in _firstChar, (nint)(uint)index /* force zero-extension */);
//        }
//    }
//
//    public int Length
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _length;
//    }
//
//    public bool IsEmpty
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _length == 0;
//    }
//
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt()
//    {
//        this = default;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt(ref readonly char ch)
//    {
//        _firstChar = ref ch;
//        _length = 1;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt(ReadOnlySpan<char> characters)
//    {
//        _firstChar = ref characters.GetPinnableReference();
//        _length = characters.Length;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt(string? str)
//    {
//        if (str is not null)
//        {
//            _firstChar = ref str.GetPinnableReference();
//            _length = str.Length;
//        }
//        else
//        {
//            this = default;
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt(params char[]? characters)
//    {
//        if (characters is not null)
//        {
//            _firstChar = ref MemoryMarshal.GetArrayDataReference(characters);
//            _length = characters.Length;
//        }
//        else
//        {
//            this = default;
//        }
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public txt(ref char firstChar, int length)
//    {
//        if (length < 0)
//            throw new ArgumentOutOfRangeException(nameof(length));
//        _firstChar = ref firstChar;
//        _length = length;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public unsafe txt(char* ptr, int length)
//    {
//        if (length < 0)
//            throw new ArgumentOutOfRangeException(nameof(length));
//        _firstChar = ref Notsafe.PtrAsRef<char>(ptr);
//        _length = length;
//    }
//
//
//    public ReadOnlySpan<char> AsSpan()
//    {
//        unsafe
//        {
//            return new text(Notsafe.InAsVoidPtr(in _firstChar), _length);
//        }
//    }
//
//    public bool Equals(txt other) => txt.Equate(this, other);
//
//    public bool Equals(txt other, StringComparison comparison)
//        => txt.Equate(this, other, comparison);
//
//    public int CompareTo(txt other)
//        => txt.Compare(this, other);
//
//    public int CompareTo(txt other, StringComparison comparison)
//        => txt.Compare(this, other, comparison);
//
//    public override bool Equals([NotNullWhen(true)] object? obj)
//    {
//        if (obj is string str)
//        {
//            return txt.Equate(this, str);
//        }
//        else if (obj is char[] chars)
//        {
//            return txt.Equate(this, chars);
//        }
//        else if (obj is char ch)
//        {
//            
//        }
//    }
//
//    public override int GetHashCode() => throw new NotImplementedException();
//
//    public override string ToString() => throw new NotImplementedException();
//}