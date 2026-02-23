using System.Reflection;
using ScrubJay.Enums;
using ScrubJay.Tests.Utilities;

namespace ScrubJay.Tests.EnumTests;

public class EnumExtensions_E_Tests
{
    public static TheoryData<object> TestEnumMembers { get; } = typeof(TestEnums)
        .GetNestedTypes(BindingFlags.Public | BindingFlags.Instance)
        .Where(static type => type.IsEnum)
        .Select(static type => Activator.CreateInstance(type)!)
        .ToTheoryData<object>();


    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void EquateWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            var equate = EnumExtensions.Equate(left, right);
            var eq = EqualityComparer<E>.Default.Equals(left, right);
            Assert.Equal(eq, equate);
        }
    }



    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void CompareWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            int eeCompare = EnumExtensions.Compare(left, right);
            int cdCompare = Comparer<E>.Default.Compare(left, right);
            Assert.Equal(cdCompare < 0, eeCompare < 0);
            Assert.Equal(cdCompare == 0, eeCompare == 0);
            Assert.Equal(cdCompare > 0, eeCompare > 0);
        }
    }

    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void LessThanWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            bool eeIs = EnumExtensions.IsLessThan(left, right);
            bool compareIs = Comparer<E>.Default.Compare(left, right) < 0;
            Assert.Equal(compareIs, eeIs);
        }
    }
    
    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void LessThanOrEqualToWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            bool eeIs = EnumExtensions.IsLessThanOrEqualTo(left, right);
            bool compareIs = Comparer<E>.Default.Compare(left, right) <= 0;
            Assert.Equal(compareIs, eeIs);
        }
    }
    
    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void GreaterThanWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            bool eeIs = EnumExtensions.IsGreaterThan(left, right);
            bool compareIs = Comparer<E>.Default.Compare(left, right) > 0;
            Assert.Equal(compareIs, eeIs);
        }
    }
    
    [Theory]
    [MemberData(nameof(TestEnumMembers))]
    public void GreaterThanOrEqualToWorks<E>(E _)
        where E : struct, Enum
    {
        var values = Enum.GetValues<E>();
        foreach (var left in values)
        foreach (var right in values)
        {
            bool eeIs = EnumExtensions.IsGreaterThanOrEqualTo(left, right);
            bool compareIs = Comparer<E>.Default.Compare(left, right) >= 0;
            Assert.Equal(compareIs, eeIs);
        }
    }

}