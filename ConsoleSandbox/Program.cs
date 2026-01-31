using System.Linq.Expressions;
using ScrubJay.Destructuring;
using ScrubJay.Universal;

var t = typeof(IList<int>);
var s = t.ToString();
var n = t.Name;
var fn = t.FullName;
var aqn = t.AssemblyQualifiedName;
var typename = TypeName.For(t);

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