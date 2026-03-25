using System.Linq.Expressions;

namespace ScrubJay.Text.Building;

public ref partial struct TextBuilder
{
    public TextBuilder Append(Expression? expression)
    {
        switch (expression)
        {
            case null:
                return this;
            case BinaryExpression binaryExpression:
                Debugger.Break();
                break;
            case BlockExpression blockExpression:
                Debugger.Break();
                break;
            case ConditionalExpression conditionalExpression:
                Debugger.Break();
                break;
            case ConstantExpression constantExpression:
                Debugger.Break();
                break;
            case DebugInfoExpression debugInfoExpression:
                Debugger.Break();
                break;
            case DefaultExpression defaultExpression:
                Debugger.Break();
                break;
            case DynamicExpression dynamicExpression:
                Debugger.Break();
                break;
            case GotoExpression gotoExpression:
                Debugger.Break();
                break;
            case IndexExpression indexExpression:
                Debugger.Break();
                break;
            case InvocationExpression invocationExpression:
                Debugger.Break();
                break;
            case LabelExpression labelExpression:
                Debugger.Break();
                break;
            case LambdaExpression lambdaExpression:
                Debugger.Break();
                break;
            case ListInitExpression listInitExpression:
                Debugger.Break();
                break;
            case LoopExpression loopExpression:
                Debugger.Break();
                break;
            case MemberExpression memberExpression:
                Debugger.Break();
                break;
            case MemberInitExpression memberInitExpression:
                Debugger.Break();
                break;
            case MethodCallExpression methodCallExpression:
                Debugger.Break();
                break;
            case NewArrayExpression newArrayExpression:
                Debugger.Break();
                break;
            case NewExpression newExpression:
                Debugger.Break();
                break;
            case ParameterExpression parameterExpression:
                Debugger.Break();
                break;
            case RuntimeVariablesExpression runtimeVariablesExpression:
                Debugger.Break();
                break;
            case SwitchExpression switchExpression:
                Debugger.Break();
                break;
            case TryExpression tryExpression:
                Debugger.Break();
                break;
            case TypeBinaryExpression typeBinaryExpression:
                Debugger.Break();
                break;
            case UnaryExpression unaryExpression:
                Debugger.Break();
                break;
            default:
                throw Ex.ArgRange(expression);
        }

        return this;
    }

    public TextBuilder Append<T>(Expression<T>? expression)
    {
        if (expression is null)
            return this;


        Debugger.Break();
        throw Ex.NotImplemented();
    }
}