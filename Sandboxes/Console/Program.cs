using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using ScrubJay.Enums;
using ScrubJay.Sandboxes.Console;
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var type = typeof(Exception);
var instanceMembers = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


var fields = instanceMembers.OfType<FieldInfo>().ToList();
var properties = instanceMembers.OfType<PropertyInfo>().ToList();
var events = instanceMembers.OfType<EventInfo>().ToList();
var constructors = instanceMembers.OfType<ConstructorInfo>().ToList();
var methods = instanceMembers.OfType<MethodInfo>().ToList();

Debugger.Break();



/*
var test = TestEnum.Four;
string name = test.GetName();
var value = test.GetValue();

var feq = test.Equals(TestEnum.Four);
var fcomp = test.CompareTo(TestEnum.Sixteen);

var hf = TestEnum.HasFlagsAttribute;

Console.WriteLine($"""
    TestEnum.Four
        Name: {name}
        Value: {value}
        Feq: {feq}
        Fcomp: {fcomp}
        HF: {hf}
    """);
    */

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
    [Extend]
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