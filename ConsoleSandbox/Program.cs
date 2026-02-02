using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using ConsoleSandbox;
using ScrubJay.Destructuring;
using ScrubJay.Reflection;
using ScrubJay.Reflection.Decompilation;
using ScrubJay.Reflection.Extensions;
using ScrubJay.Universal;

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

Debugger.Break();

return;

namespace ConsoleSandbox
{
    [return: NotNullIfNotNull(nameof(value))]
    public delegate T? CheckNotNull<T>([AllowNull, NotNull] T value);


    static class Util
    {
        public static string Teardown(Expression? expression)
        {
            return Destructure.Value(expression);
        }
    }
}