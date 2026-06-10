namespace ScrubJay.Memory;

[PublicAPI]
public static class TimeEncodingAffixExtensions
{
    private static readonly DateTime _dateOrigin = new DateTime(1970, 1, 1);

    extension(TimeEncodingAffix)
    {
        public static DateTime OriginDateTime
            => _dateOrigin;
    }
}