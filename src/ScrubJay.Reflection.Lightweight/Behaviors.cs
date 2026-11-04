#pragma warning disable CA1008

namespace ScrubJay.Reflection.Lightweight;

internal static class Behaviors
{
    private const string APP_NAME = "ScrubJay.Reflection.Lightweight";

    [Flags]
    public enum AnyGetHashCodeRefStructBehavior
    {
        Throw = 0,
        Passthrough = 1 << 0,
        Address = 1 << 1,
    }

    public static class Any
    {
        public static AnyGetHashCodeRefStructBehavior GetHashCodeRefStructBehavior
        {
            get
            {
                var data = AppContext.GetData($"{APP_NAME}.{nameof(AnyGetHashCodeRefStructBehavior)}")?.ToString();
                if (Enum.TryParse<AnyGetHashCodeRefStructBehavior>(data, out var behavior))
                    return behavior;
                return AnyGetHashCodeRefStructBehavior.Throw;
            }
        }
    }
}