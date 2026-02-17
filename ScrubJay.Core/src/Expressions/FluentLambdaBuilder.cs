// using System.Linq.Expressions;
// using System.Reflection;
//
// namespace ScrubJay.Expressions;
//
// [PublicAPI]
// public abstract class FluentLambdaBuilder<TSelf>
//     where TSelf : FluentLambdaBuilder<TSelf>
// {
//     protected TSelf _builder;
//     protected readonly Type _delegateType;
//     protected readonly ParameterExpression[] _parameters;
//     protected Expression? _body;
//
//
//     public IReadOnlyList<ParameterExpression> Parameters => _parameters;
//     public Type ReturnType { get; }
//
//     internal FluentLambdaBuilder(Type delegateType)
//     {
//         _builder = (TSelf)this;
//         _delegateType = Guard.Implements<Delegate>(delegateType);
//
//         var invokeMethod = _delegateType.GetMethod("Invoke",
//                 BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
//             .ThrowIfNull($"Cound not find Delegate '{TypeName.For(delegateType)}'s Invoke Method");
//
//         var methodParams = invokeMethod.GetParameters();
//         ReturnType = invokeMethod.ReturnParameter?.ParameterType ?? invokeMethod.ReturnType;
//
//         int paramCount = methodParams.Length;
//
//         _parameters = new ParameterExpression[paramCount];
//         for (var i = 0; i < paramCount; i++)
//         {
//             _parameters[i] = Expression.Parameter(methodParams[i].ParameterType, methodParams[i].Name);
//         }
//     }
//
//     public TSelf NameParameter(int parameterIndex, string? name)
//     {
//         int i = Guard.Index(parameterIndex, _parameters.Length);
//         var param = _parameters[i];
//         if (param.Name != name)
//         {
//             _parameters[i] = Expression.Parameter(param.Type, name);
//         }
//
//         return _builder;
//     }
//
//     public TSelf NameParameters(params string?[] names)
//     {
//         int count = _parameters.Length;
//         if (names.Length != count)
//             throw Ex.Arg(names, $"{names.Length} names specified for {count} parameters");
//         for (int i = 0; i < count; i++)
//         {
//             ParameterExpression param = _parameters[i];
//             string? name = names[i];
//             if (param.Name != name)
//             {
//                 _parameters[i] = Expression.Parameter(param.Type, name);
//             }
//         }
//
//         return _builder;
//     }
//
//     public TSelf BuildBody(Func<BodyBuilder, Expression> createBody)
//     {
//         _body = createBody(new(_parameters));
//         return _builder;
//     }
//
//     public Result<Delegate> TryCompile()
//     {
//         if (_body is null)
//             return new InvalidOperationException("Body has not been set");
//
//         LambdaExpression lambda = Expression.Lambda(_delegateType, _body, _parameters);
//         try
//         {
//             return lambda.Compile();
//         }
//         catch (Exception ex)
//         {
//             return ex;
//         }
//     }
// }
//
// public class FluentLambdaBuilder<S, D> : FluentLambdaBuilder<S>
//     where S : FluentLambdaBuilder<S, D>
//     where D : Delegate
// {
//     public FluentLambdaBuilder() : base(typeof(D)) { }
//
//     public new Result<D> TryCompile()
//     {
//         if (_body is null)
//             return new InvalidOperationException("Body has not been set");
//
//         var lambdaExpression = Expression.Lambda<D>(_body, _parameters);
//         try
//         {
//             return lambdaExpression.Compile();
//         }
//         catch (Exception ex)
//         {
//             return ex;
//         }
//     }
// }