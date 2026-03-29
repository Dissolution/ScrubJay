//namespace ScrubJay.Enhancements.Text.Pooling;
//
///// <summary>
///// An <seealso cref="ObjectPool{T}"/> for <see cref="StringBuilder"/> instances.
///// </summary>
//[PublicAPI]
//public sealed class StringBuilderPool : DefaultObjectPool<StringBuilder>
//{
//    public static StringBuilderPool Shared { get; } = new();
//
//    public StringBuilderPool() : base(new StringBuilderPooledObjectPolicy())
//    {
//
//    }
//
//    public StringBuilderPool(IPooledObjectPolicy<StringBuilder> policy) : base(policy)
//    {
//    }
//    
//
//    public string ToStringAndReturn(StringBuilder builder)
//    {
//        string str = builder.ToString();
//        base.Return(builder);
//        return str;
//    }
//}
//
//public static class StringBuilderExtensions
//{
//    extension(StringBuilder)
//    {
//        public static StringBuilder Rent() => StringBuilderPool.Shared.Get();
//    }
//
//    extension(StringBuilder? builder)
//    {
//        public void Return()
//        {
//            if (builder is not null)
//            {
//                StringBuilderPool.Shared.Return(builder);
//            }
//        }
//    }
//}