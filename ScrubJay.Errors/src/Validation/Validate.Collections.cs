namespace ScrubJay.Errors.Validation;

partial class Validate
{
    public static Result<T[]> NotEmpty<T>(
        T[]? array,
        string? info = null,
        [CallerArgumentExpression(nameof(array))]
        string? argumentName = null)
    {
        if (array is null)
            return Ex.ArgNull<T[]>(array, info, argumentName);
        if (array.Length == 0)
            return Ex.ArgEmpty(array, info, argumentName);
        return array;
    }
    
    public static Result<C> NotEmpty<C, T>(
        C? collection,
        string? info = null,
        [CallerArgumentExpression(nameof(collection))]
        string? argumentName = null)
        where C : ICollection<T>
    {
        if (collection is null)
            return Ex.ArgNull<C>(collection, info, argumentName);
        if (collection.Count == 0)
            return Ex.ArgEmpty<C>(collection, info, argumentName);
        return collection;
    }
}