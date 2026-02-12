using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using ScrubJay.Extensions;
using ScrubJay.Rendering;
using ScrubJay.Text.Building;

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

var ser = JsonSerializer.Serialize(new TextBuilder());


using var builder = new TextBuilder();
builder.Render(BindingFlags.Public |  BindingFlags.Static);
var str = builder.ToString();


Debugger.Break();

return;

namespace ConsoleSandbox
{
    [return: NotNullIfNotNull(nameof(value))]
    public delegate T? CheckNotNull<T>([AllowNull, NotNull] T value);


    static class Util
    {

    }
}