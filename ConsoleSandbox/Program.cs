#pragma warning disable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using InlineIL;
using ScrubJay.Extensions;
using ScrubJay.Reflection.Decompilation;
using ScrubJay.Rendering.Rendition5;
using ScrubJay.Sandboxes;
using ScrubJay.Text.Building;
using ScrubJay.Universal;
using ScrubJay.Universal.Tests.Internal;
using ScrubJay.Validation;
using Xunit;
using static InlineIL.IL;

var code = new TextBuilder();

foreach (var type in typeof(TestTypes).GetNestedTypes(BindingFlags.Public | BindingFlags.Instance))
{
    var csType = type.Name;

    code.AppendLine($$"""
        [Fact]
        public void Any_ToString_{{csType}}_Works()
        {
            {{csType}} instance = new();
            string? anyStr = Any.ToString(in instance);
            Assert.NotNull(anyStr);     
            string? str = instance.ToString();
            Assert.Equal(str, anyStr);
        }    
        """).NewLine();
}

var c = code.ToStringAndDispose();




Debugger.Break();

return;



namespace ScrubJay.Sandboxes
{
    [return: NotNullIfNotNull(nameof(value))]
    public delegate T? CheckNotNull<T>([AllowNull, NotNull] T value);


    public abstract class WeirdAbstractClass : IDisposable
    {
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class WeirdClass : WeirdAbstractClass, IDisposable
    {
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    static class Util
    {



        public static void OnEnum<E>(E @enum)
            where E : struct, Enum
        {

        }

        public static void Capture<T>(T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        {
            var ex = new ArgumentException(null, argumentName);
            var str = ex.ToString();
            var message = TextBuilder.New
                .Append($"Argument \"{argumentName}\": {Any.GetType<T>(argument)} = `{Any.ToString<T>(argument)}` was invalid")
                .ToStringAndDispose();
            var ex2 = new ArgumentException(message, argumentName);
            var str2 = ex2.ToString();

            var ex3 = new ArgException();
            var str3 = ex3.Message;
            Debugger.Break();
        }
    }

    public class FormattableClass : IFormattable, IRenderable
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"{nameof(FormattableClass)}({Id}, {Name})";
        }

        public void RenderTo(TextBuilder builder)
        {
            builder.Append(nameof(FormattableClass))
                .Append('(')
                .Delimit(", ", Id, Name)
                .Append(')');
            Debugger.Break();
        }
    }

    public struct FormattableStruct : IFormattable, IRenderable
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"{nameof(FormattableStruct)}({Id}, {Name})";
        }

        public void RenderTo(TextBuilder builder)
        {
            builder.Append(nameof(FormattableStruct))
                .Append('(')
                .Delimit(", ", Id, Name)
                .Append(')');
            Debugger.Break();
        }
    }
}