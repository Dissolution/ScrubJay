// cannot

#pragma warning disable CA1810

using System.Linq.Expressions;
using System.Reflection;
using ScrubJay.Expressions;

namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on <see cref="decimal"/>
/// </summary>
[PublicAPI]
public static class DecimalExtensions
{
    /// <summary>
    /// A delegate that gets the number of digits after the decimal point in a <see cref="decimal"/>
    /// </summary>
    private delegate int GetDigits(decimal dec);

    /// <summary>
    /// Compiled and stored <see cref="GetDigits"/>
    /// </summary>
    private static readonly GetDigits _getDigits;

    /// <summary>
    /// The smallest <see cref="decimal"/> value that can be represented
    /// </summary>
    /// <remarks>
    /// <c>1e-28m</c>
    /// </remarks>
    public static readonly decimal Epsilon = new(lo: 1, mid: 0, hi: 0, isNegative: false, scale: 28);

    static DecimalExtensions()
    {
        var flagsField = typeof(decimal)
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(static field => field.Name.Contains("flag", StringComparison.OrdinalIgnoreCase))
            .One();

        string flagsFieldName = flagsField.Name;

        // getDigits
        // (in dec) => ((dec.flags & ~int.MinValue) >> 16)

        var decParameter = Expression.Parameter(typeof(decimal), "dec");
        var decFlagsField = Expression.Field(decParameter, flagsFieldName);
        var negMinValue = Expression.Constant(~int.MinValue, typeof(int));
        var sixteen = Expression.Constant(16, typeof(int));
        var getDigits = Expression.RightShift(Expression.And(decFlagsField, negMinValue), sixteen);
        _getDigits = Expression.Lambda<GetDigits>(getDigits, decParameter).Compile();

        // IProducingShout lambda = default!;
        // lambda.Parameter(0)
        //     .Field(flagsFieldName)
        //     .And
        //     .Constant<int>(~int.MinValue)
        //     .RightShift
        //     .Constant<int>(16);


        //
        // _getDigits = LambdaBuilder
        //     .Build<GetDigits>()
        //     .NameParameter(0, "dec")
        //     .BuildBody(b => b
        //         .Parameters[0]
        //         .Field(flagsFieldName)
        //         .And()
        //         .Constant(147)
        //         
        //         
        //         .And(b.Constant<int>(~int.MinValue))
        //         .RightShift(b.Constant<int>(16)))
        //     .TryCompile().OkOrThrow();
    }

    extension(decimal dec)
    {
        /// <summary>
        /// Gets the count of digits after the decimal point in this <see cref="decimal"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int TrailingDigitCount() => _getDigits(dec);
    }
}