namespace ScrubJay.Validation;

partial class Guard
{
    public static bool IsTrue(bool boolean,
        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)
    {
        if (boolean)
            return boolean;
        throw Ex.Arg(boolean, "was not true", booleanName);
    }
    
    public static bool IsFalse(bool boolean,
        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)
    {
        if (!boolean)
            return boolean;
        throw Ex.Arg(boolean, "was not false", booleanName);
    }
 
    public static bool? IsTrue(bool? boolean,
        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)
    {
        if (boolean == true)
            return boolean;
        throw Ex.Arg(boolean, "was not true", booleanName);
    }
    
    public static bool? IsFalse(bool? boolean,
        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)
    {
        if (boolean == false)
            return boolean;
        throw Ex.Arg(boolean, "was not false", booleanName);
    }
    
}