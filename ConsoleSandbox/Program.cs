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


TestType.SealedClass instance = new();

string str = instance.ToString();

string str1 = RRM1(ref instance);

string str2 = RRM2(ref instance);

string str3 = RRM3(ref instance);

var methods = Any.GetType(instance)
    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    .Where(static method => method.Name == "ToString")
    .ToList();

#endif


Debugger.Break();

return;

static string RRM1(ref readonly TestType.SealedClass instance)
{
    return instance.ToString();
}

static string RRM2(ref readonly TestType.SealedClass instance)
{
//    Emit.Ldarg_0();
//    Emit.Call(MethodRef.Method(typeof(object), "ToString", returnType: typeof(string), genericParameterCount: 0, parameterTypes: []));
//    return Return<string>();
    return null;
}

static string RRM3(ref readonly TestType.SealedClass instance)
{
    Emit.Ldarg_0();
    Emit.Constrained(typeof(object));
    Emit.Callvirt(MethodRef.Method(typeof(object), "ToString", returnType: typeof(string), genericParameterCount: 0, parameterTypes: []));
    return Return<string>();
    //return null;
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