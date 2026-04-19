//namespace ScrubJay.Rendering.Rendition5;
//
///// <summary>
///// Indicates that this Method is to be used to Render Values
///// </summary>
///// <exception cref="InvalidOperationException">
///// Thrown if this Method's signature is not compatible with <see cref="RenderTo{T}"/>.
///// </exception>
//[PublicAPI]
//[AttributeUsage(AttributeTargets.Method)]
//public class RenderToMethodAttribute : Attribute;
//
//[PublicAPI]
//[AttributeUsage(AttributeTargets.Method)]
//public sealed class RenderToMethodAttribute<T> : RenderToMethodAttribute
//#if NET9_0_OR_GREATER
//where T : allows ref struct
//#endif
//;