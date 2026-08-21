namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="Result{T}"/> and <see cref="Result{T,E}"/>
/// </summary>
[PublicAPI]
public static class ResultExtensions
{
    extension<T1, T2>(in Result<(T1, T2)> result)
    {
        public bool IsOk([MaybeNullWhen(false)] out T1 item1, [MaybeNullWhen(false)] out T2 item2)
        {
            if (result.IsOk(out var tuple))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                return false;
            }
        }

        public bool IsOk(
            [MaybeNullWhen(false)] out T1 item1,
            [MaybeNullWhen(false)] out T2 item2,
            [NotNullWhen(false)] out Exception? error)
        {
            if (result.IsOk(out var tuple, out error))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                return false;
            }
        }
    }

    extension<T1, T2, T3>(in Result<(T1, T2, T3)> result)
    {
        public bool IsOk(
            [MaybeNullWhen(false)] out T1 item1,
            [MaybeNullWhen(false)] out T2 item2,
            [MaybeNullWhen(false)] out T3 item3)
        {
            if (result.IsOk(out var tuple))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                item3 = tuple.Item3;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                item3 = default;
                return false;
            }
        }
    }
}