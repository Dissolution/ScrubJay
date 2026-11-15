namespace ScrubJay.Enums;

[PublicAPI]
public static class EnumExtensions
{
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public IEnumerable<E> EnumerateFlags()
        {
            var members = Enum.GetValues<E>();
            foreach (var member in members)
            {
                if (@enum.HasFlag(member))
                    yield return member;
            }
        }
    }
}