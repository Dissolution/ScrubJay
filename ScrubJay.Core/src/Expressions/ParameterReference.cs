// using System.Linq.Expressions;
// using System.Reflection;
//
// namespace ScrubJay.Expressions;
//
// /// <summary>
// /// A way to reference a Parameter:<br/>
// /// <see cref="int"/> -- By index<br/>
// /// <see cref="string"/> -- By name<br/>
// /// <see cref="ParameterExpression"/> -- By <see cref="Expression"/> Parameter<br/>
// /// <see cref="ParameterInfo"/> -- By <see cref="MethodBase"/> Parameter<br/>
// /// </summary>
// [PublicAPI]
// public readonly struct ParameterReference :
// #if NET7_0_OR_GREATER
//     IEqualityOperators<ParameterReference, ParameterReference, bool>,
// #endif
//     IEquatable<ParameterReference>
// {
//     public static bool operator ==(ParameterReference left, ParameterReference right) => left.Equals(right);
//     public static bool operator !=(ParameterReference left, ParameterReference right) => !left.Equals(right);
//
//     public static implicit operator ParameterReference(int index) => new(index);
//
//     public static implicit operator ParameterReference(string name) => new(name);
//
//     public static implicit operator ParameterReference(ParameterExpression parameter) => new(parameter);
//
//     public static implicit operator ParameterReference(ParameterInfo parameter) => new(parameter);
//
//
//     private readonly object? _object;
//
//     public ParameterReference(int index)
//     {
//         _object = Guard.IsGrequalTo(index, 0);
//     }
//
//     public ParameterReference(string name)
//     {
//         _object = Guard.IsNotEmpty(name);
//     }
//
//     public ParameterReference(ParameterExpression parameter)
//     {
//         _object = Guard.IsNotNull(parameter);
//     }
//
//     public ParameterReference(ParameterInfo parameter)
//     {
//         _object = Guard.IsNotNull(parameter);
//     }
//
//     /*
//     public Option<int> IsIndex
//     {
//         get
//         {
//             if (_object is int index)
//                 return Some(index);
//             return None;
//         }
//     }
//
//     public Option<string> IsName => _object.Is<string>().AsOption();
//
//     public Option<ParameterExpression> IsParameterExpression => _object.Is<ParameterExpression>().AsOption();
//
//
//     public bool Equals(ParameterReference other)
//         => ObjectRelater.Default.Equate(_obj, other._obj);
//
//     public override bool Equals([NotNullWhen(true)] object? obj)
//         => obj is ParameterReference node && Equals(node);
//
//     public override int GetHashCode()
//         => Hasher.Hash(_obj);
//
//     public override string ToString()
//     {
//         if (_obj is int index)
//             return $"[{index}]";
//         if (_obj is string name)
//             return $"\"{name}\"";
//         if (_obj is ParameterExpression parameter)
//         {
//             var type = parameter.IsByRef ? parameter.Type.MakeByRefType() : parameter.Type;
//             return Build($"{type:@} {parameter.Name}");
//         }
//         return _obj?.ToString() ?? "null";
//     }
//     */
// }
