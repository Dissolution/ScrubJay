using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using ConsoleSandbox;
using ScrubJay.Destructuring;
using ScrubJay.Reflection;
using ScrubJay.Universal;


Debugger.Break();

return;

namespace ConsoleSandbox
{
    [return: NotNullIfNotNull(nameof(value))]
    public delegate T? CheckNotNull<T>([AllowNull, NotNull] T value);

    
    static class Util
    {
        public static string Teardown(Expression? expression)
        {
            return Destructure.Value(expression);
        }
    
    
    }
}