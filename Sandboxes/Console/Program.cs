using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using ScrubJay.Exceptions;
using ScrubJay.Sandboxes.Console;
using ScrubJay.Text.Building;
using ScrubJay.Universal;
using ScrubJay.Text.Utilities;
using ScrubJay.Text.Extensions;
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;


using var text = TextBuilder.Create(4);
text.Append("eat").Append("at").Append("joes").NewLine()
    .Append($"Or the jabberwock will: {args}")
    .NewLine();

string str = text.ToString();
Console.WriteLine(str);
Debugger.Break();
    


Console.WriteLine("Press enter to close this Sandbox.");
//Console.ReadLine();
Debugger.Break();
return;

namespace ScrubJay.Sandboxes.Console
{
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