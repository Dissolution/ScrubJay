#pragma warning disable

using System.ComponentModel;
using System.Runtime.Serialization;
using ScrubJay.Errors;
using ScrubJay.Text.Building;
using ScrubJay.Text.Rendering;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

int[] arr = [1, 4, 7];
arr.Reverse();

Console.WriteLine("Press enter to close this Sandbox.");
//Console.ReadLine();
Debugger.Break();
return;


namespace ScrubJay.Sandboxes.Console
{
    public ref struct TestRefStruct
    {
        public override string ToString() => "TestRefStruct";
    }


    public class RenderableThing : IRenderable
    {
        public required int Id { get; init; }
        public string? Name { get; set; }

        [SetsRequiredMembers]
        public RenderableThing()
        {
            Id = Guid.NewGuid().GetHashCode();
        }

        [SetsRequiredMembers]
        public RenderableThing(int id)
        {
            Id = id;
        }

        [SetsRequiredMembers]
        public RenderableThing(int id, string? name)
        {
            Id = id;
            Name = name;
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
            Format = null;
            Provider = null;
        }

        public FormatInfo(string? format)
        {
            Format = format;
            Provider = null;
        }

        public FormatInfo(string? format, IFormatProvider? provider)
        {
            Format = format;
            Provider = provider;
        }
    }


    internal partial class Util
    {
        public static string EnumThing<E>(E e)
            where E : struct, Enum
        {



            throw Ex.NotImplemented();
        }

        //        public static string EnumThing<E>(E left, E right)
        //            where E : struct, Enum
        //        {
        //            //var eq = left == right;
        //            
        //            
        //            
        //            throw Ex.NotImplemented();
        //        }

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