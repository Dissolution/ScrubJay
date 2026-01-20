using System.Linq.Expressions;

namespace ScrubJay.Destructuring;

public interface IDestructor<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
#if NET9_0_OR_GREATER
    static abstract TextBuilder DestructTo(TextBuilder builder, T? value, DestructureOptions options);
#endif
}

internal sealed class ParametersAndClosuresVisitor : ExpressionVisitor
{
    private static bool IsCompilerGeneratedObject(object? value)
    {
        if (value is null)
            return false;

        var type = value.GetType();

        return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
               && type.Name.Contains("DisplayClass");
    }

    public static (HashSet<ParameterExpression>, HashSet<ConstantExpression>) Process(Expression? expression)
    {
        if (expression is null)
            return ([], []);
        var visitor = new ParametersAndClosuresVisitor();
        visitor.Visit(expression);
        return (visitor._parameters, visitor._closedUpon);
    }


    private readonly HashSet<ConstantExpression> _closedUpon = [];
    private readonly HashSet<ParameterExpression> _parameters = [];

    public ParametersAndClosuresVisitor() { }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression is ConstantExpression constant &&
            IsCompilerGeneratedObject(constant.Value))
        {
            _closedUpon.Add(constant);
        }

        if (node.Expression is ParameterExpression parameter)
        {
            _parameters.Add(parameter);
        }

        return base.VisitMember(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        _parameters.Add(node);
        return base.VisitParameter(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (IsCompilerGeneratedObject(node.Value))
        {
            _closedUpon.Add(node);
        }

        return base.VisitConstant(node);
    }
}

internal sealed class ExpressionDestructor : ExpressionVisitor, IDestructor<Expression>
{
    private readonly HashSet<ParameterExpression> _parameters;
    private readonly HashSet<ConstantExpression> _closured;

    private readonly TextBuilder _builder;
    private DestructureOptions _options;

    public ExpressionDestructor(TextBuilder builder, DestructureOptions options,
        HashSet<ParameterExpression> parameters, HashSet<ConstantExpression> closured)
    {
        _builder = builder;
        _options = options;
        _parameters = parameters;
        _closured = closured;
    }

    private static bool IsCompilerGeneratedObject(object? value)
    {
        if (value is null)
            return false;

        var type = value.GetType();

        return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
               && type.Name.Contains("DisplayClass");
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value is null)
        {
            _builder.Append("null:").DestructType(node.Type);
        }
        else
        {
            if (IsCompilerGeneratedObject(node.Value))
            {
                _closured.Add(node);
            }

            _builder.Destruct(node.Value);
        }

        return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        return base.VisitMember(node);
    }

    protected override Expression VisitParameter(ParameterExpression parameterExpression)
    {
        if (_parameters.Add(parameterExpression))
        {
            _builder.If(parameterExpression.IsByRef, "ref ")
                .DestructType(parameterExpression.Type)
                .Write(' ');
        }

        _builder.Write(parameterExpression.Name);

        return base.VisitParameter(parameterExpression);
    }

    protected override Expression VisitLambda<T>(Expression<T> lambdaExpression)
    {
        Debugger.Break();
        return base.VisitLambda(lambdaExpression);
    }


    protected override Expression VisitBinary(BinaryExpression node)
    {
        return base.VisitBinary(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        return base.VisitBlock(node);
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
        return base.VisitCatchBlock(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        return base.VisitConditional(node);
    }

    protected override Expression VisitDebugInfo(DebugInfoExpression node)
    {
        return base.VisitDebugInfo(node);
    }

    protected override Expression VisitDefault(DefaultExpression node)
    {
        return base.VisitDefault(node);
    }

    protected override Expression VisitDynamic(DynamicExpression node)
    {
        return base.VisitDynamic(node);
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
        return base.VisitElementInit(node);
    }

    protected override Expression VisitExtension(Expression node)
    {
        return base.VisitExtension(node);
    }

    protected override Expression VisitGoto(GotoExpression node)
    {
        return base.VisitGoto(node);
    }

    protected override Expression VisitIndex(IndexExpression node)
    {
        return base.VisitIndex(node);
    }

    protected override Expression VisitInvocation(InvocationExpression node)
    {
        return base.VisitInvocation(node);
    }

    protected override Expression VisitLabel(LabelExpression node)
    {
        return base.VisitLabel(node);
    }

    [return: NotNullIfNotNull("node")]
    protected override LabelTarget? VisitLabelTarget(LabelTarget? node)
    {
        return base.VisitLabelTarget(node);
    }


    protected override Expression VisitListInit(ListInitExpression node)
    {
        return base.VisitListInit(node);
    }

    protected override Expression VisitLoop(LoopExpression node)
    {
        return base.VisitLoop(node);
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        return base.VisitMemberAssignment(node);
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding node)
    {
        return base.VisitMemberBinding(node);
    }

    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        return base.VisitMemberInit(node);
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        return base.VisitMemberListBinding(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
        return base.VisitMemberMemberBinding(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        return base.VisitNew(node);
    }

    protected override Expression VisitNewArray(NewArrayExpression node)
    {
        return base.VisitNewArray(node);
    }


    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
        return base.VisitRuntimeVariables(node);
    }

    protected override Expression VisitSwitch(SwitchExpression node)
    {
        return base.VisitSwitch(node);
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
        return base.VisitSwitchCase(node);
    }

    protected override Expression VisitTry(TryExpression node)
    {
        return base.VisitTry(node);
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        return base.VisitTypeBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        return base.VisitUnary(node);
    }

    [return: NotNullIfNotNull(nameof(node))]
    public override Expression? Visit(Expression? node)
    {
        return base.Visit(node);
    }

    public static TextBuilder DestructTo(TextBuilder builder, Expression? expression,
        DestructureOptions options = DestructureOptions.ForExpression)
    {
        if (expression is null)
            return builder;

        var (parameters, closed) = ParametersAndClosuresVisitor.Process(expression);

        var visitor = new ExpressionDestructor(builder, options, parameters, closed);
        visitor.Visit(expression);
        return builder;
    }
}