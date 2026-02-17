// namespace ScrubJay.Expressions;
//
// public class LambdaBuilder : FluentLambdaBuilder<LambdaBuilder>
// {
//     public LambdaBuilder(Type delegateType) : base(delegateType) { }
//
//     public static LambdaBuilder<D> Build<D>()
//         where D : Delegate
//     {
//         throw Ex.NotImplemented();
//     }
// }
//
// public class LambdaBuilder<D> : FluentLambdaBuilder<LambdaBuilder<D>, D>
//     where D : Delegate
// {
//     public LambdaBuilder() : base() { }
// }