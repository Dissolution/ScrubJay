namespace ScrubJay.Reflection.Lightweight.Duck;



public static class Quack
{
    public static ref QuackInternals.NoInstance NoInstance => ref QuackInternals.NoInstance.Instance;
    public static readonly QuackInternals.Void Void = default;


//    public bool TryGetFieldRef<I, V>(
//        [NotNullWhen(true)] 
//        FieldInfo? field, 
//        [NotNullWhen(true)]
//        out FieldRef<I, V>? fieldRef)
//    {
//        if (field is null)
//            goto FAIL;
//
//        if (!field.IsStatic)
//        {
//            Type fieldParent = field.ParentType!;
//            if (!typeof(I).IsAssignableTo(fieldParent))
//                goto FAIL;
//        }
//        
//        
//        
//        FAIL:
//        fieldRef = null;
//        return false;
//    }
    
}

[PublicAPI]
public delegate ref V FieldRef<I, V>(ref I instance);
[PublicAPI]
public delegate V FieldGetter<I, out V>(ref readonly I instance);
[PublicAPI]
public delegate void FieldSetter<I, in V>(ref I instance, V value);
[PublicAPI]
public delegate V PropertyGetter<I, out V>(ref I instance);
[PublicAPI]
public delegate void PropertySetter<I, in V>(ref I instance, V value);
[PublicAPI]
public delegate void EventAdder<I, in EH>(ref I instance, EH handler)
    where EH : Delegate;
[PublicAPI]
public delegate void EventRemover<I, in EH>(ref I instance, EH handler)
    where EH : Delegate;
[PublicAPI]
public delegate void EventRaiser<I>(ref I instance, params ReadOnlySpan<object?> args);
[PublicAPI]
public delegate I Constructor<out I>(params ReadOnlySpan<object?> args);
[PublicAPI]
public delegate R MethodInvoker<I, out R>(ref I instance, params ReadOnlySpan<object?> args);




public static class QuackInternals
{
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct NoInstance
    {
        internal static NoInstance Instance;
    }

    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct Void;
}