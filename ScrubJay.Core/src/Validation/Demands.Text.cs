// namespace ScrubJay.Testing;
//
// partial class Demands
// {
//     extension(Actual<string?> actual)
//     {
//         public Actual<string?> IsEqualTo(scoped ReadOnlySpan<char> other,
//             StringComparison comparison = StringComparison.Ordinal)
//         {
//             if (MemoryExtensions.Equals(actual.Value, other, comparison))
//                 return actual;
//             throw new ActualException(actual.Realize(), $"was not equal to \"{other}\"");
//         }
//
//         public Actual<string?> IsEqualTo(string? other, StringComparison comparison = StringComparison.Ordinal)
//         {
//             if (MemoryExtensions.Equals(actual.Value, other, comparison))
//                 return actual;
//             throw new ActualException(actual.Realize(), $"was not equal to \"{other}\"");
//         }
//     }
//
// #if NET9_0_OR_GREATER
//     extension(Actual<text> actual)
//     {
//         public Actual<text> IsEqualTo(scoped ReadOnlySpan<char> other,
//             StringComparison comparison = StringComparison.Ordinal)
//         {
//             if (MemoryExtensions.Equals(actual.Value, other, comparison))
//                 return actual;
//             throw new ActualException(actual.Realize(), $"was not equal to \"{other}\"");
//         }
//
//         public Actual<text> IsEqualTo(string? other, StringComparison comparison = StringComparison.Ordinal)
//         {
//             if (MemoryExtensions.Equals(actual.Value, other, comparison))
//                 return actual;
//             throw new ActualException(actual.Realize(), $"was not equal to \"{other}\"");
//         }
//     }
// #endif
// }