namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class BindingFlagsExtensions
{
    extension(BindingFlags)
    {
        public static BindingFlags All => BindingFlags.Instance | BindingFlags.Static |
                                          BindingFlags.Public | BindingFlags.NonPublic |
                                          BindingFlags.IgnoreCase;

        public static BindingFlags AllInstance => BindingFlags.Instance |
                                                  BindingFlags.Public | BindingFlags.NonPublic |
                                                  BindingFlags.IgnoreCase;

        public static BindingFlags AllStatic => BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.IgnoreCase;

        public static BindingFlags AllPublic => BindingFlags.Instance | BindingFlags.Static |
                                                BindingFlags.Public |
                                                BindingFlags.IgnoreCase;

        public static BindingFlags AllNonPublic => BindingFlags.Instance | BindingFlags.Static |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.IgnoreCase;
    }
}