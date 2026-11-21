using ScrubJay.Comparison;
using ScrubJay.Text;

namespace ScrubJay.Reflection.Signatures;

public class DelegateSignature :
    IEqualityOperators<DelegateSignature, DelegateSignature, bool>,
    IEquatable<DelegateSignature>
{
    public static bool operator ==(DelegateSignature? left, DelegateSignature? right) => Relate.Equate(left, right);

    public static bool operator !=(DelegateSignature? left, DelegateSignature? right) => !Relate.Equate(left, right);

    public Type ReturnType { get; }

    public Type[] ParameterTypes { get; }

    public DelegateSignature(Type? returnType, params Type[]? parameterTypes)
    {
        this.ReturnType = returnType ?? typeof(void);
        this.ParameterTypes = parameterTypes ?? [];
    }

    public bool Equals(DelegateSignature? other)
    {
        return other is not null
            && other.ReturnType == ReturnType
            && other.ParameterTypes.SequenceEqual(ParameterTypes);
    }

    public override bool Equals(object? obj) => obj is DelegateSignature delegateSignature && Equals(delegateSignature);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(ReturnType);
        hashCode.AddMany(ParameterTypes);
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        using Interpolator interpolator = new();
        interpolator.Write('(');
        interpolator.Delimit(", ", ParameterTypes, static (b, t) => b.Write(t.Alias));
        interpolator.Write(")->");
        interpolator.Write(ReturnType.Alias);
        return interpolator.ToString();
    }
}