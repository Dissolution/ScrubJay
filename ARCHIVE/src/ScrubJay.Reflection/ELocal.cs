using ScrubJay.Errors;
using ScrubJay.Errors.Validation;
using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Reflection;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct ELocal :
#if NET7_0_OR_GREATER
    IEqualityOperators<ELocal, ELocal, bool>,
#endif
    IEquatable<ELocal>,
    IEquatable<LocalVariableInfo>
{
    public static bool operator ==(ELocal left, ELocal right) => left.Equals(right);
    public static bool operator !=(ELocal left, ELocal right) => !left.Equals(right);

    public static bool IsValidLocalIndex(int index) => index is >= ushort.MinValue and < ushort.MaxValue;

    public static Result<ushort> ValidateIndex(int localIndex)
    {
        if (localIndex >= ushort.MinValue && localIndex < ushort.MaxValue)
            return (ushort)localIndex;
        return Ex.ArgRange(localIndex, "local variables must have an index in [0..65534]");
    }
    
    
    public readonly Type Type;
    public readonly ushort Index;
    public readonly bool IsPinned;
    public readonly string? Name;
    
    public bool IsShortForm => Index <= byte.MaxValue;
  
    public ELocal(ushort index, Type type, bool isPinned = false, string? name = null)
    {
        this.Index = ValidateIndex(index).OkOrThrow();
        this.Type = Demand.NotNull(type);
        this.IsPinned = isPinned;
        this.Name = name;
    }
    
    public ELocal(LocalVariableInfo local, string? name = null)
    {
        this.Index = ValidateIndex(local.LocalIndex).OkOrThrow();
        this.Type = Demand.NotNull(local.LocalType);
        this.IsPinned = local.IsPinned;
        this.Name = name;
    }

    public bool Equals(ELocal local)
    {
        return local.Index == this.Index &&
            local.Type == this.Type &&
            local.IsPinned == this.IsPinned;
    }

    public bool Equals(LocalVariableInfo? local)
    {
        return local is not null &&
            local.LocalIndex == this.Index &&
            local.LocalType == this.Type &&
            local.IsPinned == this.IsPinned;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ELocal eLocal)
            return Equals(eLocal);
        if (obj is LocalVariableInfo localVar)
            return Equals(localVar);
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Index, Type, IsPinned);

    public override string ToString()
    {
        using InterpolatedText builder = $"[{Index}]";
        if (IsPinned)
        {
            builder.Write("📌 ");
        }
        else
        {
            builder.Write(' ');
        }
        
        builder.Write(TypeName.For(Type));

        if (Name.IsNotEmpty())
        {
            builder.Write($" \"{Name}\"");
        }

        return builder.ToString();
    }
}
