using ScrubJay.Errors.Validation;

namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class DelegateExtensions
{
    extension(Delegate)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetInvokeMethod<D>()
            where D : Delegate
        {
            return typeof(D)
                .GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        }
    }
}