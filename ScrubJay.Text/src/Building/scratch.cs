//namespace scratch;
//
//public readonly record struct ValueFormatInfo<T>
//{
//    public readonly T? Value;
//    public readonly string? Format;
//    public readonly IFormatProvider? Provider;
//
//    public ValueFormatInfo(T? value, string? format = null, IFormatProvider? provider = null)
//    {
//        Value = value;
//        Format = format;
//        Provider = provider;
//    }
//
//}
//
//
//[PublicAPI]
//public abstract class TBA
//{
//    public static implicit operator TBA(char ch) => new DelegateValueTBA<char>(static (tb, c) => tb.Write(c), ch);
//    public static implicit operator TBA(string? str) => new DelegateValueTBA<string?>(static (tb, s) => tb.Write(s), str);
//    public static implicit operator TBA(Action<TextBuilder>? build) => new DelegateTBA(build);
//
//    public abstract void Invoke(TextBuilder textBuilder);
//
//
//    internal sealed class DelegateTBA : TBA
//    {
//        private readonly Action<TextBuilder>? _delegate;
//
//        public DelegateTBA(Action<TextBuilder>? @delegate)
//        {
//            _delegate = @delegate;
//        }
//
//        public override void Invoke(TextBuilder textBuilder)
//        {
//            _delegate?.Invoke(textBuilder);
//        }
//    }
//
//    internal sealed class DelegateValueTBA<T> : TBA
//    {
//        private readonly Action<TextBuilder, T?>? _delegate;
//        private readonly T? _value;
//
//        public DelegateValueTBA(Action<TextBuilder, T?>? @delegate, T? value)
//        {
//            _delegate = @delegate;
//            _value = value;
//        }
//
//        public override void Invoke(TextBuilder textBuilder)
//        {
//            _delegate?.Invoke(textBuilder, _value);
//        }
//    }
//}
//
//
//
//public abstract class TBA<T>
//{
//
//    internal static readonly Action<TextBuilder, T?> _write = static (tb, value) => tb.Write<T>(value);
//    internal static readonly Action<TextBuilder, T?> _append = static (tb, value) => tb.Append<T>(value);
//    internal static readonly Action<TextBuilder, T?> _render = static (tb, value) => tb.Render<T>(value);
//    internal static readonly Action<TextBuilder, T?> _format = static (tb, value) => tb.Format<T>(value);
//
//    public static TBA<T> Append { get; } = new DelegateTBA(_append);
//    public static TBA<T> Render { get; } = new DelegateTBA(_render);
//    public static TBA<T> Format { get; } = new DelegateTBA(_format);
//    
//    
//
//    public abstract void Invoke(TextBuilder textBuilder, T? value);
//    
//    
//    internal sealed class DelegateTBA : TBA<T>
//    {
//        private readonly Action<TextBuilder, T?>? _delegate;
//
//        public DelegateTBA(Action<TextBuilder, T?>? @delegate)
//        {
//            _delegate = @delegate;
//        }
//
//        public override void Invoke(TextBuilder textBuilder, T? value) => _delegate?.Invoke(textBuilder, value);
//    }
//    
//    internal sealed class DelegateValueTBA : TBA<T>
//    {
//        private readonly Action<TextBuilder, T?>? _delegate;
//        private readonly T? _value;
//
//        public DelegateValueTBA(Action<TextBuilder, T?>? @delegate, T? value)
//        {
//            _delegate = @delegate;
//            _value = value;
//        }
//
//        public override void Invoke(TextBuilder textBuilder, T? value)
//        {
//            _delegate?.Invoke(textBuilder, _value);
//        }
//    }
//}
//
//
