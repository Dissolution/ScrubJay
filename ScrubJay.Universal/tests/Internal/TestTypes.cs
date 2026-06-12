#pragma warning disable CA1852

namespace ScrubJay.Universal.Tests.Internal;

internal static partial class TestTypes
{
    internal static partial class NoToString
    {
        public enum TestEnum
        {
            None,
            Alfa,
            Bravo,
            Charlie,
            Delta,
        }

        [Flags]
        public enum TestFlaggedEnum
        {
            None = 0,
            Alpha = 1 << 0,
            Beta = 1 << 1,
            Gamma = 1 << 2,
            Delta = 1 << 3,
        }

        public struct TestStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestStruct)}.{nameof(ToString)}()";
        }

        public readonly struct TestReadonlyStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestReadonlyStruct)}.{nameof(ToString)}()";
        }

        public ref struct TestRefStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestRefStruct)}.{nameof(ToString)}()";
        }

        public readonly ref struct TestReadonlyRefStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestReadonlyRefStruct)}.{nameof(ToString)}()";
        }

        public record struct TestRecordStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestRecordStruct)}.{nameof(ToString)}()";
        }

        public readonly record struct TestReadonlyRecordStruct
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestReadonlyRecordStruct)}.{nameof(ToString)}()";
        }

        public class TestClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestClass)}.{nameof(ToString)}()";
        }

        public sealed class TestSealedClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestSealedClass)}.{nameof(ToString)}()";
        }

        public abstract class TestAbstractClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestAbstractClass)}.{nameof(ToString)}()";
        }

        public class TestParentClass : TestAbstractClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestParentClass)}.{nameof(ToString)}()";
        }

        public class TestGrandParentClass : TestParentClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestGrandParentClass)}.{nameof(ToString)}()";
        }

        public record class TestRecordClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestRecordClass)}.{nameof(ToString)}()";
        }

        public sealed record class TestSealedRecordClass
        {
            //public override string ToString() => $"{nameof(TestType)}.{nameof(TestSealedRecordClass)}.{nameof(ToString)}()";
        }
    }

    internal static partial class OverrideToString
    {
        public struct TestStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestStruct)}.{nameof(ToString)}()";
        }

        public readonly struct TestReadonlyStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestReadonlyStruct)}.{nameof(ToString)}()";
        }

        public ref struct TestRefStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestRefStruct)}.{nameof(ToString)}()";
        }

        public readonly ref struct TestReadonlyRefStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestReadonlyRefStruct)}.{nameof(ToString)}()";
        }

        public record struct TestRecordStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestRecordStruct)}.{nameof(ToString)}()";
        }

        public readonly record struct TestReadonlyRecordStruct
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestReadonlyRecordStruct)}.{nameof(ToString)}()";
        }

        public class TestClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestClass)}.{nameof(ToString)}()";
        }

        public sealed class TestSealedClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestSealedClass)}.{nameof(ToString)}()";
        }

        public abstract class TestAbstractClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestAbstractClass)}.{nameof(ToString)}()";
        }

        public class TestParentClass : TestAbstractClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestParentClass)}.{nameof(ToString)}()";
        }

        public class TestGrandParentClass : TestParentClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestGrandParentClass)}.{nameof(ToString)}()";
        }

        public record class TestRecordClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestRecordClass)}.{nameof(ToString)}()";
        }

        public sealed record class TestSealedRecordClass
        {
            public override string ToString() => $"{nameof(TestTypes)}.{nameof(OverrideToString)}.{nameof(TestSealedRecordClass)}.{nameof(ToString)}()";
        }
    }

}