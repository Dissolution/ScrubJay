using ScrubJay.Enums;
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;






Console.WriteLine("Press enter to close this Sandbox.");
Console.ReadLine();
return;



internal partial class Util
{
    public static void EnumThing<E>(E e)
        where E : struct, Enum
    {
        e.Format(EnumPart.Attribute, )
    }
}