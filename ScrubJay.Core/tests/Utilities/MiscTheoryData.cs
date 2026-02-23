namespace ScrubJay.Tests.Utilities;

public class MiscTheoryData : TheoryData
{
    public MiscTheoryData() : base()
    {
    }

    public MiscTheoryData(IEnumerable<object?> data) : base()
    {
        base.AddRange(data.Select(o => new TheoryDataRow(o)));
    }

    public void Add<T>(T? value)
    {
        base.Add(new TheoryDataRow(value));
    }
}

[PublicAPI]
internal static class TheoryDataExtensions
{
    public static TheoryData<T> ToTheoryData<T>(this IEnumerable<T> values)
    {
        return new TheoryData<T>(values);
    }
    
    public static MiscTheoryData ToTheoryData(this IEnumerable values)
    {
        return new MiscTheoryData(values.OfType<object?>());
    }
    
    public static MiscTheoryData ToTheoryData(this IEnumerable<object?> values)
    {
        return new MiscTheoryData(values);
    }
}