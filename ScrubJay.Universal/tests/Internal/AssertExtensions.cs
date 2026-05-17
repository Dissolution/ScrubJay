namespace ScrubJay.Universal.Tests.Internal;

internal static class AssertExtensions
{
    extension(Assert)
    {
        [AssertionMethod]
        public static void Null<T>(T? value)
        {
            Assert.True(value is null);
        }

        [AssertionMethod]
        public static void NotNull<T>(T? value)
        {
            Assert.True(value is not null);
        }
    }
}