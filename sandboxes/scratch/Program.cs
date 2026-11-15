using TextCopy;

var builder = new StringBuilder();


for (int i = 0x00; i <= 0x7F; i++)
{
    builder.Append($"\\x{Convert.ToHexString([(byte)i])}");
}

var str = builder.ToString();
Console.WriteLine(str);
ClipboardService.SetText(str);

// ---
Console.WriteLine("Press enter to close this application.");
Console.ReadLine();