using System.Reflection;

var type = typeof(int).MakeByRefType();
string str = type.ToString();

Debugger.Break();


return;

namespace ConsoleSandbox
{
    static class Util
    {
    
    
        public static bool IsGenericArgumentNullable(Type genericType, int argumentIndex)
        {
            var attr = genericType.GetCustomAttribute<NullableAttribute>();
            if (attr?.NullableFlags != null && attr.NullableFlags.Length > argumentIndex + 1)
            {
                return attr.NullableFlags[argumentIndex + 1] == 2;
            }

            return false; // Unable to determine, assume not nullable
        }
    }
}