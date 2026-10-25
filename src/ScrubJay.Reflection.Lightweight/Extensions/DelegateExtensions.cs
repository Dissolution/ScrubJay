namespace ScrubJay.Reflection.Lightweight.Extensions;

[PublicAPI]
public static class DelegateExtensions
{
    extension(Delegate)
    {
        /// <summary>
        /// Gets the <c>Invoke</c> <see cref="MethodInfo"/> for the <typeparamref name="D"/> <see langword="delegate"/>.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        public static MethodInfo GetInvokeMethod<D>()
            where D : Delegate
        {
            return typeof(D).GetMethod("Invoke",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        }
    }

    extension(Delegate? del)
    {
        [return: NotNullIfNotNull(nameof(del))]
        public MethodInfo? GetInvokeMethod()
        {
            if (del is null)
                return null;
            return del.GetType().GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        }
    }
}