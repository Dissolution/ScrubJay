using System.Dynamic;
using System.Linq.Expressions;
using ScrubJay.Testing.Things;

namespace ScrubJay.Testing.Naughty;

public sealed class LyingDynamicObject : DynamicObject
{
    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return [TestStrings.Word(3), TestStrings.Word(8), TestStrings.Word(16)];
    }

    public override DynamicMetaObject GetMetaObject(Expression parameter)
    {
        return null!;
    }

    public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TryConvert(ConvertBinder binder, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TryCreateInstance(CreateInstanceBinder binder, object?[]? args, [NotNullWhen(true)] out object? result)
    {
        result = null!;
        return true;
    }

    public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
    {
        return true;
    }

    public override bool TryDeleteMember(DeleteMemberBinder binder)
    {
        return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TryInvoke(InvokeBinder binder, object?[]? args, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = null;
        return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        return true;
    }

    public override bool TryUnaryOperation(UnaryOperationBinder binder, out object? result)
    {
        result = null;
        return true;
    }
}