namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class BindingFlagsExtensions
{
    extension(BindingFlags)
    {
        public static BindingFlags AllVisibilities => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        
        public static BindingFlags AllPublic => BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        
        public static BindingFlags AllNonPublic => BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        
        public static BindingFlags AllInstance => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        public static BindingFlags AllStatic => BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }
}