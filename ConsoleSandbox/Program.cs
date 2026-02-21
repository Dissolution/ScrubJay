#pragma warning disable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ConsoleSandbox;
using ScrubJay.Rendering.Rendition5;
using ScrubJay.Text.Building;
using ScrubJay.Validation;

/*
var methods = AppDomain
    .CurrentDomain
    .GetAssemblies()
    .SelectMany(static assembly => assembly.GetTypes())
    .SelectMany(static type => type.GetMethods(BindingFlags.All))
    .Where(method => method.GetMethodBody()?.GetILAsByteArray() is not null)
    //.OrderBy(method => method.GetMethodBody()!.GetILAsByteArray()!.Length)
    .ToList();

foreach (var method in methods)
{
    var d = new DecompiledMethod(method);
    var str = d.ToString();
    Debugger.Break();
}
*/


var x = Ex.Arg<int>(147, "now");

Debugger.Break();

return;

namespace ConsoleSandbox
{
    [return: NotNullIfNotNull(nameof(value))]
    public delegate T? CheckNotNull<T>([AllowNull, NotNull] T value);


    static class Util
    {

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