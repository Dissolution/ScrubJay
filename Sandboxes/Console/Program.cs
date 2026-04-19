using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using ScrubJay.Sandboxes.Console;
using ScrubJay.Text;
using ScrubJay.Text.Building;
using TBA = ScrubJay.Sandboxes.Console.TBA;


Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

using var textBuilder = new TextBuilder();


textBuilder.IfNotNull(args, TBA.Render, TBA.Append("shit"));


string str = textBuilder.ToString();
Console.WriteLine(str);
Debugger.Break();



Console.WriteLine("Press enter to close this Sandbox.");
//Console.ReadLine();
Debugger.Break();
return;


namespace ScrubJay.Sandboxes.Console
{
    public readonly record struct FormatInfo
    {
        public static implicit operator FormatInfo(string? format) => new(format);
        public static implicit operator FormatInfo((string?, IFormatProvider?) tuple) => new(tuple.Item1, tuple.Item2);

        public static readonly FormatInfo None = new();

        public readonly string? Format;

        public readonly IFormatProvider? Provider;

        public FormatInfo()
        {
            this.Format = null;
            this.Provider = null;
        }

        public FormatInfo(string? format)
        {
            this.Format = format;
            this.Provider = null;
        }

        public FormatInfo(string? format, IFormatProvider? provider)
        {
            this.Format = format;
            this.Provider = provider;
        }
    }

 





    public delegate void TBADel<T>(TextBuilder textBuilder, T value);

    public static class MiscExtensions
    {
        extension(TextBuilder tb)
        {
            public TextBuilder DoThing<T>(T value, TBADel<T> tba)
            {
                Debugger.Break();
                return tb;
            }
        }

    }

    /*


    [PublicAPI]
    public readonly struct TBA
    {
        private static readonly TBA _format = new(FormatInfo.None);


        public static readonly TBA None = default;

        public static readonly TBA NewLine = Create(static tb => tb.NewLine());


        public static readonly TBA Append = new(nameof(Append));

        public static readonly TBA Render = new(nameof(Render));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TBA Format() => _format;

        public static TBA Format(string? format)
            => new(new FormatInfo(format));

        public static TBA Format(string? format, IFormatProvider? formatProvider)
            => new(new FormatInfo(format, formatProvider));


        public static TBA Create(Action<TextBuilder> build)
        {
            return new(build);
        }


        internal readonly object? _box;

        public TBA()
        {
            _box = null;
        }

        private TBA(Action<TextBuilder> build)
        {
            _box = build;
        }

        private TBA(string specialAction)
        {
            _box = specialAction;
        }

        private TBA(FormatInfo formatInfo)
        {
            _box = formatInfo;
        }

        public void Invoke(TextBuilder textBuilder)
        {
            if (_box is Action<TextBuilder> action)
            {
                action(textBuilder);
            }
        }
    }


    public readonly partial struct TBA<T>
    {
        public static implicit operator Action<TextBuilder, T>(TBA<T> tba) => tba.Invoke;
        public static implicit operator TBA<T>(Action<TextBuilder, T>? buildItem) => Create(buildItem);
        public static implicit operator TBA<T>(TBA tba) => FromTBA(tba);


        internal static TBA<T> FromTBA(TBA tba)
        {
            object? box = tba._box;
            if (box is string specialAction)
            {
                if (specialAction == nameof(TBA.Append))
                    return Append;
                if (specialAction == nameof(TBA.Render))
                    return Render;
                if (specialAction == nameof(TBA.Format))
                    return _format;
                throw new InvalidOperationException("Unknown Special Action");
            }
            if (box is FormatInfo formatInfo)
            {
                if (formatInfo.Provider is null)
                {
                    if (formatInfo.Format is null)
                        return _format;
                    return Format(formatInfo.Format);
                }
                return Format(formatInfo.Format, formatInfo.Provider);
            }
            if (box is Action<TextBuilder> action)
            {
                // do this instead!
                return new((tb, value) => action(tb));
            }
            if (box is null)
                return None;
            throw new InvalidOperationException("Unknown TBA");
        }

        internal static readonly TBA<T> _format = new(static (tb, value) => tb.Format<T>(value));

        public static readonly TBA<T> None = new(static (tb, value) => { });

        public static readonly TBA<T> Append = new(static (tb, value) => tb.Write<T>(value));
        public static readonly TBA<T> Render = new(static (tb, value) => tb.Render<T>(value));

        public static TBA<T> Format()
            => _format;

        public static TBA<T> Format(string? format)
            => new((tb, value) => tb.Format<T>(value, format));

        public static TBA<T> Format(string? format, IFormatProvider? formatProvider)
            => new((tb, value) => tb.Format<T>(value, format, formatProvider));


        public static TBA<T> Create(Action<TextBuilder, T>? buildItem) => new(buildItem);


        private readonly Action<TextBuilder, T>? _action;

        private TBA(Action<TextBuilder, T>? action)
        {
            _action = action;
        }



        public void Invoke(TextBuilder textBuilder, T value)
        {
            if (_action is not null)
            {
                _action.Invoke(textBuilder, value);
            }
        }
    }
    */



    internal static class RuntimeBuilder
    {
        private static readonly AssemblyBuilder _assembly =
            AssemblyBuilder.DefineDynamicAssembly(new("ScrubJay.Reflection.Runtime"), AssemblyBuilderAccess.Run);
        private static readonly ModuleBuilder _module =
            _assembly.DefineDynamicModule("ScrubJay.Refection.Runtime");

        public static Type CreateTBADelegateType()
        {
            var delegateTypeBuilder = _module.DefineType("TBADel`1",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass,
                typeof(MulticastDelegate));
            var genericTypeParameters = delegateTypeBuilder.DefineGenericParameters("T");


            var ctor = delegateTypeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                [typeof(object), typeof(IntPtr)]
            );
            ctor.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            // Invoke: your actual delegate signature — body is runtime-provided
            var invoke = delegateTypeBuilder.DefineMethod(
                "Invoke",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(void),
                [typeof(TextBuilder), genericTypeParameters[0]]
            );
            invoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            return delegateTypeBuilder.CreateTypeInfo()!;

        }


    }


    internal partial class Util
    {
        public static string EnumThing<E>(E e)
            where E : struct, Enum
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.Static;



            return bf.ToString();
        }

    }

    [Flags]
    //[Extend]
    public enum TestEnum : int
    {
        [Description("DESC")]
        Zero = 0,
        [EnumMember(Value = "1")]
        One = 1 << 0,
        [DataMember(Name = "2")]
        Two = 1 << 1,
        Four = 1 << 2,
        Eight = 1 << 3,
        Sixteen = 1 << 4,
    }
}