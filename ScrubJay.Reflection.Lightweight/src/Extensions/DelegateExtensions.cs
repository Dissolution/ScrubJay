namespace ScrubJay.Reflection.Lightweight;

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
}