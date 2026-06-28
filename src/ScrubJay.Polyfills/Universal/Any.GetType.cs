namespace ScrubJay.Polyfills.Universal;

partial class Any
{
    public static Type GetType<T>(in T? value)
    {
        if (value is null)
            return typeof(T);
        return value.GetType();
    }

#if NET9_0_OR_GREATER
    public static Type GetType<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return typeof(T);
    }
#endif
}