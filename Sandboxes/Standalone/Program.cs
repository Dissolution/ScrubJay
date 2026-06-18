


using System.Diagnostics;


var types = new List<Type?>();

types.Add(Type.GetType("System.Func", false));
for (var i = 0; i <= 20; i++)
{
    var name = $"System.Func`{i}";
    types.Add(Type.GetType(name, false));
}


Debugger.Break();