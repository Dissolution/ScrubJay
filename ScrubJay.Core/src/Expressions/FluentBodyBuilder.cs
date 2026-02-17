// using System.Linq.Expressions;
// using System.Reflection;
//
// namespace ScrubJay.Expressions;
//
// [PublicAPI]
// public abstract class FluentBodyBuilder<B>
//     where B : FluentBodyBuilder<B>
// {
//     protected readonly B _builder;
//
//     public IReadOnlyList<ParameterExpression> Parameters { get; }
//     public Type ReturnType { get; }
//
//     internal FluentBodyBuilder(ParameterExpression[] parameters, Type returnType)
//     {
//         _builder = (B)this;
//         Parameters = parameters;
//         this.ReturnType = returnType;
//     }
//
//     private Expression Resolve(ParameterReference paramRef)
//     {
//         if (paramRef.IsIndex.IsSome(out var index))
//         {
//             int offset = Guard.Index(index, Parameters.Count);
//             return Parameters[offset];
//         }
//         else if (paramRef.IsName.IsSome(out var name))
//         {
//             var param = Parameters.OneOrDefault(p => ((string?)p.Name).Equate(name));
//             if (param is not null)
//                 return param;
//             throw Ex.Arg(paramRef, $"There is no parameter named \"{name}\"");
//         }
//         else
//         {
//             throw new UnreachableException();
//         }
//     }
//
// #region Call
//
//     public MethodCallExpression Call(MethodInfo staticMethod, params ParameterReference[] parameters)
//     {
//         if (!staticMethod.IsStatic)
//             throw Ex.Arg(staticMethod);
//         var methodArgs = parameters.SelectToArray(Resolve);
//         MethodCallExpression callExpr = Expression.Call(staticMethod, methodArgs);
//         return callExpr;
//     }
//
//     public MethodCallExpression Call(MethodInfo staticMethod, IReadOnlyList<ParameterExpression> parameters)
//     {
//         if (!staticMethod.IsStatic)
//             throw Ex.Arg(staticMethod);
//         var methodArgs = parameters.OfType<Expression>().ToArray();
//         MethodCallExpression callExpr = Expression.Call(staticMethod, methodArgs);
//         return callExpr;
//     }
//
//
//     public MethodCallExpression Call(ParameterReference instance, MethodInfo instanceMethod, params ParameterReference[] args)
//     {
//         if (instanceMethod.IsStatic)
//             throw Ex.Arg(instanceMethod);
//
//         var methodArgs = args.Select(Resolve).ToArray();
//         MethodCallExpression callExpr = Expression.Call(Resolve(instance), instanceMethod, methodArgs);
//         return callExpr;
//     }
//
// #endregion
//
//     public ConstantExpression Constant<T>(T? value)
//     {
//         return Expression.Constant(value, typeof(T));
//     }
// }