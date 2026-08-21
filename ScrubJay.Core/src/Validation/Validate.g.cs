#nullable enable

namespace ScrubJay.Validation;



[PublicAPI]

public static partial class Validate

{

    public static Result<T> IsNotNull<T>([AllowNull] T actual,

        [CallerArgumentExpression(nameof(actual))] string? actualName = null)

    {

        if (actual is not null)

            return actual;

        return Ex.ArgNull(actualName);

    }

}
