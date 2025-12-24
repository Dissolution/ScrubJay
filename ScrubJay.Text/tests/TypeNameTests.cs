using System;
using System.Collections.Generic;
using System.Text;
using ScrubJay.Interpolated;
using ScrubJay.Testing;
using ScrubJay.Text;
using Xunit;

namespace ScrubJay.Interpolated.Tests;

/// <summary>
/// Comprehensive unit tests for TypeName utility using TheoryData
/// </summary>
public class TypeNameTests
{
#region Type Aliases

    public static TheoryData<Type, string> TypeAliasData => new()
    {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(nint), "nint" },
        { typeof(nuint), "nuint" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(object), "object" },
        { typeof(void), "void" },
        { typeof(Tuple), "()" },
        { typeof(ValueTuple), "()" },
    };

    [Theory]
    [MemberData(nameof(TypeAliasData))]
    public void TypeAliasesWork(Type type, string expected)
    {
        Demand.That(TypeName.For(type)).IsEqualTo(expected);
    }

#endregion

#region Nullable Types

    public static TheoryData<Type, string> NullableStructData => new()
    {
        { typeof(int?), "int?" },
        { typeof(bool?), "bool?" },
        { typeof(double?), "double?" },
        { typeof(decimal?), "decimal?" },
        { typeof(byte?), "byte?" },
        { typeof(long?), "long?" },
        { typeof(char?), "char?" },
        { typeof(DateTime?), "DateTime?" },
        { typeof(Guid?), "Guid?" },
    };

    [Theory]
    [MemberData(nameof(NullableStructData))]
    public void NullableWorks(Type type, string expected)
    {
        Demand.That(TypeName.For(type)).IsEqualTo(expected);
    }

#endregion

#region Array Types

    public static TheoryData<Type, string> ArrayTypeData => new()
    {
        { typeof(int[]), "int[]" },
        { typeof(string[]), "string[]" },
        { typeof(object[]), "object[]" },
        { typeof(int[,]), "int[,]" },
        { typeof(int[,,]), "int[,,]" },
        { typeof(int[,,,]), "int[,,,]" },
        { typeof(string[,]), "string[,]" },
        { typeof(double[,,,,,,,,,,,,,,,,,,,,,,,,,,]), "double[,,,,,,,,,,,,,,,,,,,,,,,,,,]" },
    };

    [Theory]
    [MemberData(nameof(ArrayTypeData))]
    public void NDArraysWork(Type type, string expected)
    {
        Demand.That(TypeName.For(type)).IsEqualTo(expected);
    }

    public static TheoryData<Type, string> ComplexArrayTestData => new()
    {
        // Arrays of arrays
        { typeof(int[][]), "int[][]" },
        { typeof(int[][][][][]), "int[][][][][]" },

        // Mixed multi-dimensional arrays
        { typeof(int[][,]), "int[][,]" },
        { typeof(int[,][]), "int[,][]" },
        { typeof(int[,,][,]), "int[,,][,]" },
    };
    
    [Theory]
    [MemberData(nameof(ComplexArrayTestData))]
    public void ComplexArraysWork(Type type, string expected)
    {
        Demand.That(TypeName.For(type)).IsEqualTo(expected);
    }

#endregion

#region Generic Types

    public static TheoryData<Type, string> GenericTypeData => new()
    {
        { typeof(List<int>), "List<int>" },
        { typeof(List<string>), "List<string>" },
        { typeof(Dictionary<int, string>), "Dictionary<int, string>" },
        { typeof(Dictionary<string, object>), "Dictionary<string, object>" },
        { typeof(IEnumerable<int>), "IEnumerable<int>" },
        { typeof(IList<double>), "IList<double>" },
        { typeof(HashSet<string>), "HashSet<string>" },
        { typeof(Dictionary<string, List<int>>), "Dictionary<string, List<int>>" },
        { typeof(List<List<int>>), "List<List<int>>" },
        { typeof(Dictionary<int, Dictionary<string, bool>>), "Dictionary<int, Dictionary<string, bool>>" },
    };

    [Theory]
    [MemberData(nameof(GenericTypeData))]
    public void For_GenericTypes_ReturnsCorrectFormat(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Tuple Types

    public static TheoryData<Type, string> TupleTypeData => new()
    {
        { typeof((int, string)), "(int, string)" },
        { typeof((int, int)), "(int, int)" },
        { typeof((string, bool, double)), "(string, bool, double)" },
        { typeof((int, string, bool, double)), "(int, string, bool, double)" },
        { typeof((byte, short, int, long, float)), "(byte, short, int, long, float)" },
        { typeof(Tuple<int>), "(int)" },
        { typeof(Tuple<int, string>), "(int, string)" },
        { typeof(Tuple<int, string, bool>), "(int, string, bool)" },
        { typeof(ValueTuple<int>), "(int)" },
        { typeof(ValueTuple<int, string>), "(int, string)" },
        { typeof(ValueTuple<int, string, bool>), "(int, string, bool)" },
    };

    [Theory]
    [MemberData(nameof(TupleTypeData))]
    public void For_TupleTypes_ReturnsParenthesisFormat(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Pointer Types

    public static TheoryData<Type, string> PointerTypeData => new()
    {
        { typeof(int*), "int*" },
        { typeof(byte*), "byte*" },
        { typeof(void*), "void*" },
        { typeof(char*), "char*" },
        { typeof(double*), "double*" },
        { typeof(int**), "int**" },
        { typeof(byte**), "byte**" },
        { typeof(void**), "void**" },
    };

    [Theory]
    [MemberData(nameof(PointerTypeData))]
    public void For_PointerTypes_ReturnsAsterisk(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Complex Combinations

    public static TheoryData<Type, string> ComplexTypeData => new()
    {
        // Arrays of generics
        { typeof(List<int>[]), "List<int>[]" },
        { typeof(Dictionary<int, string>[]), "Dictionary<int, string>[]" },

        // Generics with arrays
        { typeof(List<int[]>), "List<int[]>" },
        { typeof(Dictionary<string, int[]>), "Dictionary<string, int[]>" },

        // Nullable in generics
        { typeof(List<int?>), "List<int?>" },
        { typeof(Dictionary<string, bool?>), "Dictionary<string, bool?>" },

        // Arrays of nullable
        { typeof(int?[]), "int?[]" },
        { typeof(bool?[]), "bool?[]" },

        // Multi-dimensional arrays of generics
        { typeof(List<int>[,]), "List<int>[,]" },
        { typeof(Dictionary<int, string>[,,]), "Dictionary<int, string>[,,]" },

        // Nested generics with nullable
        { typeof(List<Dictionary<string, int?>>), "List<Dictionary<string, int?>>" },
        { typeof(Dictionary<int, List<string?>>), "Dictionary<int, List<string?>>" },

        // Tuples with generics
        { typeof((List<int>, string)), "(List<int>, string)" },
        { typeof((int, Dictionary<string, bool>)), "(int, Dictionary<string, bool>)" },

        // Tuples with nullable
        { typeof((int?, string)), "(int?, string)" },
        { typeof((bool?, double?, string)), "(bool?, double?, string)" },

        // Tuples with arrays
        { typeof((int[], string[])), "(int[], string[])" },
        { typeof((int[,], bool)), "(int[,], bool)" },

        // Arrays of tuples
        { typeof((int, string)[]), "(int, string)[]" },
        { typeof((bool, double, char)[,]), "(bool, double, char)[,]" },

        // Generics with tuples
        { typeof(List<(int, string)>), "List<(int, string)>" },
        { typeof(Dictionary<int, (string, bool)>), "Dictionary<int, (string, bool)>" },

        // Pointer arrays
        { typeof(int*[]), "int*[]" },

        // Triple nesting
        { typeof(List<Dictionary<string, List<int>>>), "List<Dictionary<string, List<int>>>" },

        // Crazy combo: array of nullable generic with tuple
        { typeof(List<(int?, string)>[]), "List<(int?, string)>[]" },

        // Even crazier: multi-dimensional array of generic dictionary with nullable tuple values
        { typeof(Dictionary<string, (int?, bool?)>[,]), "Dictionary<string, (int?, bool?)>[,]" },
    };

    [Theory]
    [MemberData(nameof(ComplexTypeData))]
    public void For_ComplexTypes_ReturnsCorrectFormat(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Nested Types

    public static TheoryData<Type, string> NestedTypeData => new()
    {
        { typeof(OuterClass.InnerClass), "TypeNameTests.OuterClass.InnerClass" },
        { typeof(OuterClass.InnerStruct), "TypeNameTests.OuterClass.InnerStruct" },
        { typeof(OuterClass.InnerEnum), "TypeNameTests.OuterClass.InnerEnum" },
        { typeof(OuterClass.InnerClass.DeeplyNestedClass), "TypeNameTests.OuterClass.InnerClass.DeeplyNestedClass" },
        { typeof(OuterGeneric<int>.InnerClass), "TypeNameTests.OuterGeneric<int>.InnerClass" },
        { typeof(OuterGeneric<string>.InnerClass), "TypeNameTests.OuterGeneric<string>.InnerClass" },
        { typeof(OuterGeneric<int>.InnerGeneric<string>), "TypeNameTests.OuterGeneric<int>.InnerGeneric<string>" },
        {
            typeof(OuterGeneric<List<int>>.InnerGeneric<Dictionary<string, bool>>),
            "TypeNameTests.OuterGeneric<List<int>>.InnerGeneric<Dictionary<string, bool>>"
        },
    };

    [Theory]
    [MemberData(nameof(NestedTypeData))]
    public void For_NestedTypes_ReturnsCorrectFormat(Type type, string expected)
    {
        string typeName = TypeName.For(type);
        if (typeName != expected)
            Debugger.Break();
        Assert.Equal(expected, typeName);
    }

#endregion

#region Edge Cases

    [Fact]
    public void For_NullType_ReturnsNull()
    {
        Assert.Equal("null", TypeName.For(null));
    }

    [Fact]
    public void AppendType_WithStringBuilder_AppendsCorrectly()
    {
        var sb = new StringBuilder("Type: ");
        sb.AppendType(typeof(int));

        Assert.Equal("Type: int", sb.ToString());
    }

    [Fact]
    public void AppendType_NullType_AppendsNull()
    {
        var sb = new StringBuilder();
        sb.AppendType(null);

        Assert.Equal("null", sb.ToString());
    }

    [Fact]
    public void AppendType_ChainedCalls_WorksCorrectly()
    {
        var sb = new StringBuilder();
        sb.AppendType(typeof(int))
            .Append(" and ")
            .AppendType(typeof(string))
            .Append(" and ")
            .AppendType(typeof(bool));

        Assert.Equal("int and string and bool", sb.ToString());
    }

    public static TheoryData<Type, string> CustomTypeData => new()
    {
        { typeof(TestStruct), "TypeNameTests.TestStruct" },
        { typeof(TestEnum), "TypeNameTests.TestEnum" },
        { typeof(TestClass), "TypeNameTests.TestClass" },
        { typeof(ITestInterface), "TypeNameTests.ITestInterface" },
        { typeof(TestDelegate), "TypeNameTests.TestDelegate" },
    };

    [Theory]
    [MemberData(nameof(CustomTypeData))]
    public void For_CustomTypes_ReturnsTypeName(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Reference Types

    public static TheoryData<Type, string> ReferenceTypeData => new()
    {
        { typeof(int).MakeByRefType(), "int&" },
        { typeof(string).MakeByRefType(), "string&" },
        { typeof(bool).MakeByRefType(), "bool&" },
        { typeof(double).MakeByRefType(), "double&" },
        { typeof(TestStruct).MakeByRefType(), "TypeNameTests.TestStruct&" },
        { typeof(List<int>).MakeByRefType(), "List<int>&" },
    };

    [Theory]
    [MemberData(nameof(ReferenceTypeData))]
    public void For_ByRefTypes_ReturnsAmpersand(Type type, string expected)
    {
        string typeName = TypeName.For(type);

        Assert.Equal(expected, typeName);
    }

#endregion

#region Action and Func Types

    public static TheoryData<Type, string> ActionFuncTypeData => new()
    {
        { typeof(Action), "Action" },
        { typeof(Action<int>), "Action<int>" },
        { typeof(Action<int, string>), "Action<int, string>" },
        { typeof(Func<int>), "Func<int>" },
        { typeof(Func<int, string>), "Func<int, string>" },
        { typeof(Func<int, string, bool>), "Func<int, string, bool>" },
        { typeof(Predicate<int>), "Predicate<int>" },
    };

    [Theory]
    [MemberData(nameof(ActionFuncTypeData))]
    public void For_ActionAndFuncTypes_ReturnsCorrectFormat(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Extreme Edge Cases

    public static TheoryData<Type, string> ExtremeEdgeCaseData => new()
    {
        // Nullable of nullable-containing generic
        { typeof(List<int?>[]), "List<int?>[]" },

        // 7-element tuple (max without nesting)
        { typeof((int, int, int, int, int, int, int)), "(int, int, int, int, int, int, int)" },

        // 8-element tuple (uses TRest)
        { typeof((int, int, int, int, int, int, int, int)), "(int, int, int, int, int, int, int, int)" },


        // Pointer to pointer to array
        { typeof(int[]*), "int[]*" },

        // Generic with multiple type parameters all complex
        {
            typeof(Dictionary<List<int?>, Dictionary<string, bool?[]>>),
            "Dictionary<List<int?>, Dictionary<string, bool?[]>>"
        },
    };

    [Theory]
    [MemberData(nameof(ExtremeEdgeCaseData))]
    public void For_ExtremeEdgeCases_ReturnsCorrectFormat(Type type, string expected)
    {
        Assert.Equal(expected, TypeName.For(type));
    }

#endregion

#region Test Helper Types

    public struct TestStruct
    {
        public int Value;
    }

    public enum TestEnum
    {
        Value1,
        Value2
    }

    public class TestClass { }

    public interface ITestInterface { }

    public delegate void TestDelegate();

    public class OuterClass
    {
        public class InnerClass
        {
            public class DeeplyNestedClass { }
        }

        public struct InnerStruct { }

        public enum InnerEnum
        {
            Value
        }
    }

    public class OuterGeneric<T>
    {
        public class InnerClass { }

        public class InnerGeneric<U> { }
    }

#endregion
}