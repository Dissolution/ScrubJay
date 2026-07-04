//#pragma warning disable CS8500
//
//namespace ScrubJay.Universal.Tests;
//
//public class TypeRendererTests
//{
//    [Fact]
//    public void NullWorks()
//    {
//        Assert.Equal("〈null〉", Type.Render(null));
//    }
//
//#region C# type aliases
//
//    public static TheoryData<Type, string> TypeAliasesData { get; } =
//    [
//        (typeof(byte), "byte"),
//        (typeof(sbyte), "sbyte"),
//        (typeof(short), "short"),
//        (typeof(ushort), "ushort"),
//        (typeof(int), "int"),
//        (typeof(uint), "uint"),
//        (typeof(long), "long"),
//        (typeof(ulong), "ulong"),
//        (typeof(nint), "nint"),
//        (typeof(nuint), "nuint"),
//        (typeof(float), "float"),
//        (typeof(double), "double"),
//        (typeof(decimal), "decimal"),
//        (typeof(bool), "bool"),
//        (typeof(char), "char"),
//        (typeof(string), "string"),
//        (typeof(object), "object"),
//        (typeof(void), "void"),
//        (typeof(ValueTuple), "()"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(TypeAliasesData))]
//    public void TypeAliasesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Pointers
//
//    public static TheoryData<Type, string> PointerTypeData { get; } =
//    [
//        (typeof(void*), "void*"),
//        (typeof(byte*), "byte*"),
//        (typeof(char*), "char*"),
//        (typeof(void**), "void**"),
//        (typeof(void***), "void***"),
//        (typeof(void********), "void********"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(PointerTypeData))]
//    public void PointersWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region References
//
//    public static TheoryData<Type, string> ReferenceTypeData { get; } =
//    [
//        (typeof(int).MakeByRefType(), "int&"),
//        (typeof(string).MakeByRefType(), "string&"),
//        (typeof(Span<double>).MakeByRefType(), "Span<double>&"),
//        (typeof(double).MakeByRefType(), "double&"),
//        (typeof(TestStruct).MakeByRefType(), "TypeRendererTests.TestStruct&"),
//        (typeof(List<int>).MakeByRefType(), "List<int>&"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(ReferenceTypeData))]
//    public void ReferencesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Arrays
//
//    public static TheoryData<Type, string> ArrayTypeData { get; } =
//    [
//        (typeof(int[]), "int[]"),
//        (typeof(string[]), "string[]"),
//        (typeof(byte[,]), "byte[,]"),
//        (typeof(double*[,,]), "double*[,,]"),
//        (typeof(long[,,,]), "long[,,,]"),
//        (typeof(double[,,,,,,,,,,,,,,,,,,,,,,,,,,]), "double[,,,,,,,,,,,,,,,,,,,,,,,,,,]"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(ArrayTypeData))]
//    public void NDArraysWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//    public static TheoryData<Type, string> ComplexArrayTestData { get; } =
//    [
//        // Arrays of arrays
//        (typeof(int[][]), "int[][]"),
//        (typeof(int[][][][][]), "int[][][][][]"),
//
//        // Mixed multi-dimensional arrays
//        (typeof(int[][,]), "int[][,]"),
//        (typeof(int[,][]), "int[,][]"),
//        (typeof(int[,,,][,][,,]), "int[,,,][,][,,]"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(ComplexArrayTestData))]
//    public void ComplexArraysWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Nullable Types
//
//    public static TheoryData<Type, string> NullableData { get; } =
//    [
//        (typeof(int?), "int?"),
//        (typeof(bool?), "bool?"),
//        (typeof(double?), "double?"),
//        (typeof(decimal?), "decimal?"),
//        (typeof(byte?), "byte?"),
//        (typeof(long?), "long?"),
//        (typeof(char?), "char?"),
//        (typeof(DateTime?), "DateTime?"),
//        (typeof(Guid?), "Guid?"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(NullableData))]
//    public void NullableWorks(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Nested Types
//
//    public static TheoryData<Type, string> NestedTypeData { get; } =
//    [
//        (typeof(OuterClass.InnerClass), "TypeRendererTests.OuterClass.InnerClass"),
//        (typeof(OuterClass.InnerStruct), "TypeRendererTests.OuterClass.InnerStruct"),
//        (typeof(OuterClass.InnerEnum), "TypeRendererTests.OuterClass.InnerEnum"),
//        (typeof(OuterClass.InnerClass.DeeplyNestedClass), "TypeRendererTests.OuterClass.InnerClass.DeeplyNestedClass"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(NestedTypeData))]
//    public void NestedTypesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Generic Types
//
//    public static TheoryData<Type, string> GenericTypeData { get; } =
//    [
//        // Open types
//        (typeof(List<>), "List<>"),
//        (typeof(IDictionary<,>), "IDictionary<,>"),
//
//        (typeof(List<int>), "List<int>"),
//        (typeof(Dictionary<int, string>), "Dictionary<int, string>"),
//
//        (typeof(Dictionary<string, List<int>>), "Dictionary<string, List<int>>"),
//        (typeof(List<HashSet<IEnumerable<Guid>>>), "List<HashSet<IEnumerable<Guid>>>"),
//        (typeof(Dictionary<int, Dictionary<string, bool>>), "Dictionary<int, Dictionary<string, bool>>"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(GenericTypeData))]
//    public void GenericTypesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region NestedGenericTypes
//
//#region Nested Types
//
//    public static TheoryData<Type, string> NestedGenericTypesData { get; } = new()
//                                                                             {
//                                                                                 (typeof(OuterGeneric<int>.InnerClass), "TypeRendererTests.OuterGeneric<int>.InnerClass"),
//                                                                                 (typeof(OuterGeneric<string>.InnerClass), "TypeRendererTests.OuterGeneric<string>.InnerClass"),
//                                                                                 (typeof(OuterGeneric<int>.InnerGeneric<string>), "TypeRendererTests.OuterGeneric<int>.InnerGeneric<string>"),
//                                                                                 (typeof(OuterGeneric<List<int>>.InnerGeneric<Dictionary<string, bool>>),
//                                                                                  "TypeRendererTests.OuterGeneric<List<int>>.InnerGeneric<Dictionary<string, bool>>"), };
//
//    [Theory]
//    [MemberData(nameof(NestedGenericTypesData))]
//    public void NestedGenericTypesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#endregion
//
//#region Tuple Types
//
//    public static TheoryData<Type, string> TupleTypeData { get; } =
//    [
//        (typeof((int, string)), "(int, string)"),
//        (typeof((int, int)), "(int, int)"),
//        (typeof((string, bool, double)), "(string, bool, double)"),
//        (typeof((int, string, bool, double)), "(int, string, bool, double)"),
//        (typeof((byte, short, int, long, float)), "(byte, short, int, long, float)"),
//
//        (typeof(Tuple<int>), "(int)"),
//        (typeof(Tuple<int, string>), "(int, string)"),
//        (typeof(Tuple<int, string, bool>), "(int, string, bool)"),
//
//        (typeof(ValueTuple<int>), "(int)"),
//        (typeof(ValueTuple<int, string>), "(int, string)"),
//        (typeof(ValueTuple<int, string, bool>), "(int, string, bool)"),
//
//        // Max Length without TRest
//        (typeof(ValueTuple<char, bool, string, DateTime, TimeSpan, Guid, object>), "(char, bool, string, DateTime, TimeSpan, Guid, object)"),
//
//        // With TRest
//        (typeof((sbyte, byte, short, ushort, int, uint, long, ulong, float, double, decimal)), "(sbyte, byte, short, ushort, int, uint, long, ulong, float, double, decimal)"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(TupleTypeData))]
//    public void TuplesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Complex Combinations
//
//    public static TheoryData<Type, string> ComplexTypeData { get; } =
//    [
//        // Arrays of generics
//        (typeof(List<int>[]), "List<int>[]"),
//        (typeof(Dictionary<int, string>[]), "Dictionary<int, string>[]"),
//
//        // Generics with arrays
//        (typeof(List<int[]>), "List<int[]>"),
//        (typeof(Dictionary<string, int[]>), "Dictionary<string, int[]>"),
//
//        // Nullable in generics
//        (typeof(List<int?>), "List<int?>"),
//        (typeof(Dictionary<string, bool?>), "Dictionary<string, bool?>"),
//
//        // Arrays of nullable
//        (typeof(int?[]), "int?[]"),
//        (typeof(bool?[]), "bool?[]"),
//
//        // Multi-dimensional arrays of generics
//        (typeof(List<int>[,]), "List<int>[,]"),
//        (typeof(Dictionary<int, string>[,,]), "Dictionary<int, string>[,,]"),
//
//        // Nested generics with nullable
//        (typeof(List<Dictionary<string, int?>>), "List<Dictionary<string, int?>>"),
//        (typeof(Dictionary<int, List<string?>>), "Dictionary<int, List<string>>"),
//
//        // Tuples with generics
//        (typeof((List<int>, string)), "(List<int>, string)"),
//        (typeof((int, Dictionary<string, bool>)), "(int, Dictionary<string, bool>)"),
//
//        // Tuples with nullable
//        (typeof((int?, string)), "(int?, string)"),
//        (typeof((bool?, double?, string)), "(bool?, double?, string)"),
//
//        // Tuples with arrays
//        (typeof((int[], string[])), "(int[], string[])"),
//        (typeof((int[,], bool)), "(int[,], bool)"),
//
//        // Arrays of tuples
//        (typeof((int, string)[]), "(int, string)[]"),
//        (typeof((bool, double, char)[,]), "(bool, double, char)[,]"),
//
//        // Generics with tuples
//        (typeof(List<(int, string)>), "List<(int, string)>"),
//        (typeof(Dictionary<int, (string, bool)>), "Dictionary<int, (string, bool)>"),
//
//        // Pointer arrays
//        (typeof(int*[]), "int*[]"),
//
//        // Triple nesting
//        (typeof(List<Dictionary<string, List<int>>>), "List<Dictionary<string, List<int>>>"),
//
//        // Crazy combo: array of nullable generic with tuple
//        (typeof(List<(int?, string)>[]), "List<(int?, string)>[]"),
//
//        // Even crazier: multi-dimensional array of generic dictionary with nullable tuple values
//        (typeof(Dictionary<string, (int?, bool?)>[,]), "Dictionary<string, (int?, bool?)>[,]"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(ComplexTypeData))]
//    public void ComplexTypesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Delegate Types
//
//    private delegate char? DoThing(int i, string? str);
//
//    private delegate R DoThing<in A, in B, out R>(A a, B b);
//
//    public static TheoryData<Type, string> DelegateTypesData { get; } =
//    [
//        (typeof(Action), "Action"),
//        (typeof(Action<int>), "Action<int>"),
//        (typeof(Action<int, string>), "Action<int, string>"),
//        (typeof(Func<int>), "Func<int>"),
//        (typeof(Func<int, string>), "Func<int, string>"),
//        (typeof(Func<int, string, bool>), "Func<int, string, bool>"),
//        (typeof(Predicate<int>), "Predicate<int>"),
//        (typeof(DoThing), "TypeRendererTests.DoThing"),
//        (typeof(DoThing<int, string, char?>), "TypeRendererTests.DoThing<int, string, char?>"),
//    ];
//
//    [Theory]
//    [MemberData(nameof(DelegateTypesData))]
//    public void DelegateTypesWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#region Combinations
//
//    public static TheoryData<Type, string> CombinationTypesData
//        =>
//        [
//            (typeof(List<int?>[]), "List<int?>[]"),
//            (typeof(int[]*), "int[]*"),
//            (typeof(Dictionary<List<int?>, Dictionary<string, bool?[]>>), "Dictionary<List<int?>, Dictionary<string, bool?[]>>"),
//        ];
//
//    [Theory]
//    [MemberData(nameof(CombinationTypesData))]
//    public void CombinationsWork(Type type, string expected)
//    {
//        Assert.Equal(expected, Type.Render(type));
//    }
//
//#endregion
//
//#pragma warning disable
//
//#region Test Helper Types
//
//    // ReSharper disable ClassNeverInstantiated.Global
//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable UnusedTypeParameter
//    // ReSharper disable UnusedType.Global
//    internal struct TestStruct
//    {
//        public int Value;
//    }
//
//    internal enum TestEnum
//    {
//        Value1,
//        Value2,
//    }
//
//    internal class TestClass { }
//
//    internal interface ITestInterface { }
//
//    internal delegate void TestDelegate();
//
//    internal class OuterClass
//    {
//        internal class InnerClass
//        {
//            public class DeeplyNestedClass { }
//        }
//
//        internal struct InnerStruct { }
//
//        internal enum InnerEnum
//        {
//            Value,
//        }
//    }
//
//    internal class OuterGeneric<T>
//    {
//        internal class InnerClass { }
//
//        internal class InnerGeneric<U> { }
//    }
//
//#endregion
//}