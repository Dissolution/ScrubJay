using System.Linq.Expressions;
using System.Reflection;
using ConsoleSandbox;
using ScrubJay.Destructuring;
using ScrubJay.Universal;

int a = 147;
string b = "TRJ";


var str = Util.Teardown((Guid g) => g.ToString() + b + "ABC");

Debugger.Break();


return;

namespace ConsoleSandbox
{
    static class Util
    {
        public static string Teardown(Expression? expression)
        {
            return Destructure.Value(expression);
        }
    
    
    }
}