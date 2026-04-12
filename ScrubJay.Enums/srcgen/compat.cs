#pragma warning disable all
// ReSharper disable All

namespace System.Runtime.CompilerServices;

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class InterpolatedStringHandlerAttribute : Attribute;

[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class InterpolatedStringHandlerArgumentAttribute : Attribute
{
    public string[] Arguments { get; }

    public InterpolatedStringHandlerArgumentAttribute(string argument)
    {
        this.Arguments = [argument];
    }
        
    public InterpolatedStringHandlerArgumentAttribute(params string[] arguments)
    {
        this.Arguments = arguments;
    }
}