#nullable enable

namespace ScrubJay.Validation;



partial class Validate

{

    public static Result IsTrue(bool boolean,

        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)

    {

        if (boolean)

            return boolean;

        return Ex.Arg(boolean, "was not true", booleanName);

    }

    

    public static Result IsFalse(bool boolean,

        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)

    {

        if (!boolean)

            return boolean;

        return Ex.Arg(boolean, "was not false", booleanName);

    }

 

    public static Result<bool?> IsTrue(bool? boolean,

        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)

    {

        if (boolean == true)

            return boolean;

        return Ex.Arg(boolean, "was not true", booleanName);

    }

    

    public static Result<bool?> IsFalse(bool? boolean,

        [CallerArgumentExpression(nameof(boolean))] string? booleanName = null)

    {

        if (boolean == false)

            return boolean;

        return Ex.Arg(boolean, "was not false", booleanName);

    }

    

}
