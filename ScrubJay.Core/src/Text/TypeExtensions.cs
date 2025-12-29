// using System.Reflection;
//
// namespace ScrubJay.Interpolated;
//
// internal static class TypeExtensions
// {
//     extension(Type? type)
//     {
//         public NullabilityState Nullability
//         {
//             get
//             {
//                 if (type is null)
//                     return NullabilityState.Nullable;
//
//                 if (type.IsValueType)
//                 {
//                     if (Nullable.GetUnderlyingType(type) is not null)
//                         return NullabilityState.Nullable;
//                     return NullabilityState.NotNull;
//                 }
//
//                 return NullabilityState.Unknown;
//             }
//         }
//
//         public NullabilityState[] GenericTypesNullability
//         {
//             get
//             {
//                 if (type is null)
//                     return [];
//
// #if NET8_0_OR_GREATER
//                 var nullableAttr = type.GetCustomAttribute<NullableAttribute>();
//                 var attrs = Attribute.GetCustomAttributes(type, true);
//                 if (nullableAttr is not null)
//                 {
//                     var flags = nullableAttr.NullableFlags;
//                     return Unsafe.As<byte[], NullabilityState[]>(ref flags);
//                 }
// #endif
//
//                 return [];
//             }
//         }
//
//         public GenericType[] GetGenericTypes()
//         {
//             if (type is null)
//                 return [];
//
//             var genericTypeArguments = type.GenericTypeArguments;
//             int count = genericTypeArguments.Length;
//             var nullabilityStates = type.GenericTypesNullability;
//             var genericTypes = new GenericType[count];
//
//             for (var i = 0; i < count; i++)
//             {
//                 var genericType = genericTypeArguments[i];
//
//                 NullabilityState genericTypeNullability = getNullability(i);
//                 NullabilityState typeNullability = genericType.Nullability;
//
//                 NullabilityState nullability;
//                 if (genericTypeNullability >= typeNullability)
//                     nullability = genericTypeNullability;
//                 else
//                     nullability = typeNullability;
//
//                 genericTypes[i] = new(genericType, nullability);
//             }
//
//             return genericTypes;
//
//             NullabilityState getNullability(int i)
//             {
//                 i += 1;
//                 if (nullabilityStates.Length > i)
//                 {
//                     return nullabilityStates[i];
//                 }
//
//                 return NullabilityState.Unknown;
//             }
//         }
//     }
// }