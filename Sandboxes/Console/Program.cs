using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using ScrubJay.Exceptions;
using ScrubJay.Sandboxes.Console;
using ScrubJay.Text.Building;
using ScrubJay.Text.Rendering;



Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

using var tb = TextBuilder.Rent();

var thing = new int[] { 1, 4, 7 };
tb.Render(thing);
tb.NewLines(2);

var thing2 = new RenderableThing(147) { Name = "TRJ" };
tb.Render(thing2);
tb.NewLines(2);

text thing3 = "ABC";

var exceptional = Ex.Arg(thing3);



var checkpoint = tb.ToString();
Console.WriteLine(checkpoint);
Debugger.Break();






Console.WriteLine("Press enter to close this Sandbox.");
//Console.ReadLine();
Debugger.Break();
return;


namespace ScrubJay.Sandboxes.Console
{
    public class RenderableThing : IRenderable
    {
        public required int Id { get; init; }
        public string? Name { get; set; } = null;

        [SetsRequiredMembers]
        public RenderableThing(int id)
        {
            Id = id;
        }

        public void RenderTo(TextBuilder builder)
        {
            builder.Append($"Id: {Id}  Name: {Name:@}");
        }
    }
    
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