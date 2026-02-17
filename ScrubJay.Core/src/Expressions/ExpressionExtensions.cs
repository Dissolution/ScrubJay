using System.Linq.Expressions;

namespace ScrubJay.Expressions;


public interface IShout;

public interface IProducingShout : IShout
{
    IExtendingShout Parameter(int index);
    
    IExtendingShout Constant<T>(T? value);
}

public interface IExtendingShout : IShout
{
    IExtendingShout Field(string name);
    
    IProducingShout And { get; }
    
    IProducingShout RightShift { get; }
}

