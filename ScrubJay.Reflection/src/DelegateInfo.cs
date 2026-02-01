namespace ScrubJay.Reflection;

public class DelegateInfo
{
    public static DelegateInfo For<D>()
        where D : Delegate
    {
        var type = typeof(D);
        Debu
    }
    
    public Type ReturnType { get; set; }
    
    public Type[] ParameterTypes { get; set; }
}