// using System.Linq.Expressions;
//
// namespace ScrubJay.ARCHIVE.Destructuring;
//
// partial class Destructure
// {
//     internal static string ToOp(this ExpressionType expressionType)
//     {
//         string op = expressionType switch
//         {
//             ExpressionType.Add => "+",
//             ExpressionType.AddChecked => "(checked)+",
//             ExpressionType.And => "&",
//             ExpressionType.AndAlso => "&&",
//             ExpressionType.ArrayLength => "len()",
//             ExpressionType.ArrayIndex => "[]",
//             ExpressionType.Call => "call()",
//             ExpressionType.Coalesce => "coalesce()",
//             ExpressionType.Conditional => "conditional()",
//             ExpressionType.Constant => "const",
//             ExpressionType.Convert => "convert()",
//             ExpressionType.ConvertChecked => "convert_checked()",
//             ExpressionType.Divide => "/",
//             ExpressionType.Equal => "==",
//             ExpressionType.ExclusiveOr => "XOR",
//             ExpressionType.GreaterThan => ">",
//             ExpressionType.GreaterThanOrEqual => ">=",
//             ExpressionType.Invoke => "invoke()",
//             ExpressionType.Lambda => "lambda()",
//             ExpressionType.LeftShift => "<<",
//             ExpressionType.LessThan => "<",
//             ExpressionType.LessThanOrEqual => "<=",
//             ExpressionType.ListInit => "list_init()",
//             ExpressionType.MemberAccess => "member.access",
//             ExpressionType.MemberInit => "member_init()",
//             ExpressionType.Modulo => "%",
//             ExpressionType.Multiply => "*",
//             ExpressionType.MultiplyChecked => "ch_*",
//             ExpressionType.Negate => "-",
//             ExpressionType.UnaryPlus => "+",
//             ExpressionType.NegateChecked => "ch_-",
//             ExpressionType.New => "new()",
//             ExpressionType.NewArrayInit => "new[]",
//             ExpressionType.NewArrayBounds => "new[](bounds)",
//             ExpressionType.Not => "!",
//             ExpressionType.NotEqual => "!=",
//             ExpressionType.Or => "|",
//             ExpressionType.OrElse => "||",
//             ExpressionType.Parameter => "param()",
//             ExpressionType.Power => "^",
//             ExpressionType.Quote => "\"",
//             ExpressionType.RightShift => ">>",
//             ExpressionType.Subtract => "-",
//             ExpressionType.SubtractChecked => "(ch)-",
//             ExpressionType.TypeAs => "(type_as)",
//             ExpressionType.TypeIs => "(type_is)",
//             ExpressionType.Assign => "<==",
//             ExpressionType.Block => "{ }",
//             ExpressionType.DebugInfo => "DEBUG_INFO",
//             ExpressionType.Decrement => "--",
//             ExpressionType.Dynamic => "(dynamic)",
//             ExpressionType.Default => "(default)",
//             ExpressionType.Extension => "EXTENSION",
//             ExpressionType.Goto => "goto",
//             ExpressionType.Increment => "++",
//             ExpressionType.Index => "[]",
//             ExpressionType.Label => "LABEL",
//             ExpressionType.RuntimeVariables => "RUNTIME_VARS",
//             ExpressionType.Loop => "loop",
//             ExpressionType.Switch => "switch",
//             ExpressionType.Throw => "throw",
//             ExpressionType.Try => "try",
//             ExpressionType.Unbox => "unbox()",
//             ExpressionType.AddAssign => "+=",
//             ExpressionType.AndAssign => "&=",
//             ExpressionType.DivideAssign => "/=",
//             ExpressionType.ExclusiveOrAssign => "XOR=",
//             ExpressionType.LeftShiftAssign => ">>=",
//             ExpressionType.ModuloAssign => "%=",
//             ExpressionType.MultiplyAssign => "*=",
//             ExpressionType.OrAssign => "|=",
//             ExpressionType.PowerAssign => "^=",
//             ExpressionType.RightShiftAssign => ">>=",
//             ExpressionType.SubtractAssign => "-=",
//             ExpressionType.AddAssignChecked => "(ch)+=",
//             ExpressionType.MultiplyAssignChecked => "(ch)*=",
//             ExpressionType.SubtractAssignChecked => "(ch)-=",
//             ExpressionType.PreIncrementAssign => "++=",
//             ExpressionType.PreDecrementAssign => "--=",
//             ExpressionType.PostIncrementAssign => "=++",
//             ExpressionType.PostDecrementAssign => "=--",
//             ExpressionType.TypeEqual => "type_equal()",
//             ExpressionType.OnesComplement => "~",
//             ExpressionType.IsTrue => "(true)",
//             ExpressionType.IsFalse => "(false)",
//             _ => throw Ex.UndefinedEnum(expressionType),
//         };
//         return op;
//     }
//
//     extension(TextBuilder builder)
//     {
//         internal TextBuilder DestructConstantExpression(ConstantExpression? constantExpression, DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (constantExpression is null)
//                 return builder;
//
//             Type type = constantExpression.Type;
//             object? value = constantExpression.Value;
//
//             if (value is null)
//             {
//                 builder.Append('(').DestructMember(type).Append(")null");
//             }
//             else
//             {
//                 builder.Destruct(value);
//             }
//
//             return builder;
//         }
//         
//         public TextBuilder DestructBinaryExpression(
//             BinaryExpression? binaryExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (binaryExpression is null)
//                 return builder;
//
//             if (binaryExpression.Method is not null)
//             {
//                 builder.DestructMember(binaryExpression.Method,
//                         DestructureOptions.DeclaringType | DestructureOptions.Name | DestructureOptions.GenericTypes)
//                     .Append('(')
//                     .DestructExpression(binaryExpression.Left, options)
//                     .Append(", ")
//                     .DestructExpression(binaryExpression.Right, options)
//                     .Append(')');
//             }
//             else
//             {
//                 var type = binaryExpression.Type;
//                 var nodeType = binaryExpression.NodeType;
//                 var conversion = binaryExpression.Conversion;
//                 
//                 Debugger.Break();
//             }
//             
//             return builder;
//         }
//         
//         public TextBuilder DestructLambdaExpression(
//             LambdaExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (expression is null)
//                 return builder;
//
//             builder.Append('(')
//                 .Delimit(", ", expression.Parameters, (tb, p) => tb.DestructParameterExpression(p))
//                 .Append(") => ")
//                 .DestructExpression(expression.Body, options.WithoutFlag(DestructureOptions.Parameters));
//             
//             return builder;
//         }
//         
//         public TextBuilder DestructMemberExpression(
//             MemberExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (expression is null)
//                 return builder;
//             
//
//             if (expression.Expression is not null)
//             {
//                 builder.DestructExpression(expression.Expression, options)
//                     .Append('.');
//             }
//             
//             builder.DestructMember(expression.Member, DestructureOptions.Simple);
//             
//             return builder;
//         }
//         
//         public TextBuilder DestructMethodCallExpression(
//             MethodCallExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (expression is null)
//                 return builder;
//
//             if (expression.Object is not null)
//             {
//                 builder.DestructExpression(expression.Object, options)
//                     .Write('.');
//             }
//             
//             builder.DestructMember(expression.Method, DestructureOptions.Name | DestructureOptions.GenericTypes | DestructureOptions.Parameters);
//             
//             if (expression.Arguments.Count > 0)
//                 Debugger.Break();
//             
//             return builder;
//         }
//         
//         public TextBuilder DestructParameterExpression(
//             ParameterExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             if (expression is null)
//                 return builder;
//
//             if (options.HasFlags(DestructureOptions.Parameters))
//             {
//                 builder.If(expression.IsByRef, "ref ")
//                     .DestructType(expression.Type)
//                     .Append(' ');
//             }
//             
//             builder.Append(expression.Name);
//             
//             return builder;
//         }
//         
//         
//         
//         public TextBuilder DestructBlockExpression(
//             BlockExpression? blockExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructConditionalExpression(
//             ConditionalExpression? conditionalExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructDebugInfoExpression(
//             DebugInfoExpression? debugInfoExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructDefaultExpression(
//             DefaultExpression? defaultExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructDynamicExpression(
//             DynamicExpression? dynamicExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructExpression<T>(
//             Expression<T>? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructGotoExpression(
//             GotoExpression? gotoExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructIndexExpression(
//             IndexExpression? indexExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructInvocationExpression(
//             InvocationExpression? invocationExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructLabelExpression(
//             LabelExpression? labelExpression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructListInitExpression(
//             ListInitExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructLoopExpression(
//             LoopExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//         
//         public TextBuilder DestructMemberInitExpression(
//             MemberInitExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructNewArrayExpression(
//             NewArrayExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructNewExpression(
//             NewExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//       
//
//         public TextBuilder DestructRuntimeVariablesExpression(
//             RuntimeVariablesExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructSwitchExpression(
//             SwitchExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructTryExpression(
//             TryExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructTypeBinaryExpression(
//             TypeBinaryExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         public TextBuilder DestructUnaryExpression(
//             UnaryExpression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             Debugger.Break();
//             return builder;
//         }
//
//         
//         public TextBuilder DestructExpression(Expression? expression,
//             DestructureOptions options = DestructureOptions.ForExpression)
//         {
//             switch (expression)
//             {
//                 case null:
//                     return builder;
//                 case BinaryExpression binaryExpression:
//                     return builder.DestructBinaryExpression(binaryExpression, options);
//                 case BlockExpression blockExpression:
//                     return builder.DestructBlockExpression(blockExpression, options);
//                 case ConditionalExpression conditionalExpression:
//                     return builder.DestructConditionalExpression(conditionalExpression, options);
//                 case ConstantExpression constantExpression:
//                     return builder.DestructConstantExpression(constantExpression, options);
//                 case DebugInfoExpression debugInfoExpression:
//                     return builder.DestructDebugInfoExpression(debugInfoExpression, options);
//                 case DefaultExpression defaultExpression:
//                     return builder.DestructDefaultExpression(defaultExpression, options);
//                 case DynamicExpression dynamicExpression:
//                     return builder.DestructDynamicExpression(dynamicExpression, options);
//                 case GotoExpression gotoExpression:
//                     return builder.DestructGotoExpression(gotoExpression, options);
//                 case IndexExpression indexExpression:
//                     return builder.DestructIndexExpression(indexExpression, options);
//                 case InvocationExpression invocationExpression:
//                     return builder.DestructInvocationExpression(invocationExpression, options);
//                 case LabelExpression labelExpression:
//                     return builder.DestructLabelExpression(labelExpression, options);
//                 case LambdaExpression lambdaExpression:
//                     return builder.DestructLambdaExpression(lambdaExpression, options);
//                 case ListInitExpression listInitExpression:
//                     return builder.DestructListInitExpression(listInitExpression, options);
//                 case LoopExpression loopExpression:
//                     return builder.DestructLoopExpression(loopExpression, options);
//                 case MemberExpression memberExpression:
//                     return builder.DestructMemberExpression(memberExpression, options);
//                 case MemberInitExpression memberInitExpression:
//                     return builder.DestructMemberInitExpression(memberInitExpression, options);
//                 case MethodCallExpression methodCallExpression:
//                     return builder.DestructMethodCallExpression(methodCallExpression, options);
//                 case NewArrayExpression newArrayExpression:
//                     return builder.DestructNewArrayExpression(newArrayExpression, options);
//                 case NewExpression newExpression:
//                     return builder.DestructNewExpression(newExpression, options);
//                 case ParameterExpression parameterExpression:
//                     return builder.DestructParameterExpression(parameterExpression, options);
//                 case RuntimeVariablesExpression runtimeVariablesExpression:
//                     return builder.DestructRuntimeVariablesExpression(runtimeVariablesExpression, options);
//                 case SwitchExpression switchExpression:
//                     return builder.DestructSwitchExpression(switchExpression, options);
//                 case TryExpression tryExpression:
//                     return builder.DestructTryExpression(tryExpression, options);
//                 case TypeBinaryExpression typeBinaryExpression:
//                     return builder.DestructTypeBinaryExpression(typeBinaryExpression, options);
//                 case UnaryExpression unaryExpression:
//                     return builder.DestructUnaryExpression(unaryExpression, options);
//                 default:
//                     Debugger.Break();
//                     return builder;
//             }
//         }
//     }
// }