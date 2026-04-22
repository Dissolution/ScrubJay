namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class Render
{
    public const char FORMAT = '@';

    public static string Type(Type type) => type.Render();

    public static string Type<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => typeof(T).Render();

    public static string Type<T>(T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Any.GetType<T>(in instance).Render();

    public static string Value<T>(T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => value.Render();
}