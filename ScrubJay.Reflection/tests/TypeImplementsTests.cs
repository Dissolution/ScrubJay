#pragma warning disable CA1812

// ReSharper disable InvokeAsExtensionMethod
using TypeExtensions = ScrubJay.Reflection.Extensions.TypeExtensions;
using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection.Tests;

public class TypeExtensionsTests
{
    private class TestList<T> : List<T>, IList<T>;

    private sealed class NestedTestList<T> : TestList<T>, IList<T>;

    internal static Type?[] Types { get; } =
    [
        null,
        typeof(void),
        typeof(byte),
        typeof(void*),
        typeof(object),
        typeof(ValueType),
        typeof(TypeExtensions),
        typeof(short[]),
        typeof(int[,]),
        typeof(long[][]),
        typeof(IEnumerable),
        typeof(IList<>),
        typeof(IDictionary<,>),
        typeof(List<char>),
        typeof(List<>),
        typeof(TestList<object>),
        typeof(NestedTestList<DateTime>),
        typeof(AttributeTargets),
        typeof((int Id, string Name)),
        (new
        {
            Id = 3,
            Name = "TJ",
        }).GetType(),
    ];

    public static TheoryData<Type?> TestTypes { get; } = new TheoryData<Type>(Types);
    
    public static TheoryData<Type?, Type?> CombinedTestTypes { get; } = new MatrixTheoryData<Type, Type>(Types, Types);
    
    // support for testing
    private static T? GetDefault<T>() => default(T);
    
    [Theory]
    [MemberData(nameof(CombinedTestTypes))]
    public void OnlyNullImplementsNull(Type? left, Type? right)
    {
        if (left is null || right is null)
        {
            if (left is null && right is null)
            {
                Assert.True(TypeExtensions.Implements(left, right));
            }
            else
            {
                Assert.False(TypeExtensions.Implements(left, right));
            }
        }
    }

    [Theory]
    [MemberData(nameof(TestTypes))]
    public void AllTypesImplementThemselves(Type? type)
    {
        Assert.True(TypeExtensions.Implements(type, type));
    }

    [Theory]
    [MemberData(nameof(TestTypes))]
    public void MostTypesImplementObject(Type? type)
    {
        if (type is null) return;
        
        Assert.True(TypeExtensions.Implements<object>(type));
    }

    [Theory]
    [MemberData(nameof(TestTypes))]
    public void TypesImplementTheirBaseClasses(Type? type)
    {
        var baseTypes = type.GetBaseTypes();
        foreach (var bt in baseTypes)
        {
            Assert.True(TypeExtensions.Implements(type, bt));
        }
    }

    [Theory]
    [MemberData(nameof(TestTypes))]
    public void TypesImplementTheirInterfaces(Type? type)
    {
        var interfaceTypes = type?.GetInterfaces() ?? [];
        foreach (var it in interfaceTypes)
        {
            Assert.True(TypeExtensions.Implements(type, it));
        }
    }
    
    [Theory]
    [MemberData(nameof(TestTypes))]
    public void TypesImplementTheirOwnGenericTypeDefinition(Type? type)
    {
        if (type.GenericTypeDefinition.IsSome(out var gtd))
        {
            Assert.True(type.Implements(gtd));
        }
    }
    
    [Theory]
    [MemberData(nameof(TestTypes))]
    public void TypesImplementAllInheritedGenericTypeDefinitions(Type? type)
    {
        if (type is null) return;

        List<Type> gtds = [];
        if (type.GenericTypeDefinition.IsSome(out var gtd))
        {
            gtds.Add(gtd);
        }
        foreach (var bt in type.GetBaseTypes())
        {
            if (bt.GenericTypeDefinition.IsSome(out gtd))
            {
                gtds.Add(gtd);
            }
        }
        foreach (var it in type.GetInterfaces())
        {
            if (it.GenericTypeDefinition.IsSome(out gtd))
            {
                gtds.Add(gtd);
            }
        }

        foreach (var genericTypeDefinition in gtds)
        {
            Assert.True(type.Implements(genericTypeDefinition));
        }
    }
    
    [Theory]
    [MemberData(nameof(CombinedTestTypes))]
    public void ImplementsAgreesWithTypeIsAssignableTo(Type? left, Type? right)
    {
        var implements = TypeExtensions.Implements(left, right);
        var isAssignableTo = left?.IsAssignableTo(right) == true;
        Assert.Equal(isAssignableTo, implements);
    }
}