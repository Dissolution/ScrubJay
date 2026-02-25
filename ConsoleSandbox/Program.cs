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
using ScrubJay.Validation;
using static InlineIL.IL;

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


#if NET9_0_OR_GREATER


TestType.RefStruct instance = new();

//string str = instance.ToString();

//string str2 = Any.ToString(ref instance);

string str3 = RefReadonlyMethod(ref instance);

string str4 = RRM2(ref instance);

var methods = typeof(TestType.RefStruct).GetMethods(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);

#endif


Debugger.Break();

return;

static string RefReadonlyMethod(ref readonly TestType.RefStruct rs)
//static string RefReadonlyMethod(ref readonly ReadOnlySpan<byte> rs)
{
    //return rs.ToString();
    return null!;
}

static string RRM2(ref readonly TestType.RefStruct rs)
//static string RRM2(ref readonly ReadOnlySpan<byte> rs)
{
    Emit.Ldarg_0();
    Emit.Constrained(typeof(TestType.RefStruct));
    Emit.Callvirt(MethodRef.Method(typeof(TestType.RefStruct), "ToString"));
    return Return<string>();
}


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