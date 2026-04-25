//using System.Reflection;
//
//namespace ScrubJay.Text.Rendering;
//
///// <summary>
///// The <c>delegate</c> shape of a compatible <see cref="MethodInfo"/> that can be used with the <see cref="RenderToMethodAttribute"/>.
///// </summary>
///// <typeparam name="T">
///// The <see cref="Type"/> of the <paramref name="value"/> to be rendered.<br/>
///// <b>Note:</b> You may apply <i>any</i> generic type constraints to your actual method.
///// </typeparam>
//[PublicAPI]
//public delegate void RenderTo<in T>(T value, TextBuilder builder)
//#if NET9_0_OR_GREATER
//    where T : allows ref struct
//#endif
//;