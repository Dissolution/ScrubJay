using System.Reflection;

namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on <see cref="Type"/> and <see cref="TypeInfo"/>
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    extension(Type)
    {
        public static Type GetType(object? obj)
        {
            if (obj is null) return typeof(object);
            return obj.GetType();
        }

#if NET9_0_OR_GREATER
        public static Type GetType<T>(T? instance = default)
            where T : allows ref struct
        {
            return typeof(T);
        }
#else
        public static Type GetType<T>(T? instance = default)
        {
            return instance?.GetType() ?? typeof(T);
        }
#endif
    }

    extension(Type? type)
    {
        public bool IsRef => type is not null && (type.IsByRef || type.IsByRefLike);

#if NETSTANDARD2_0 || NETFRAMEWORK
        public bool IsByRefLike => false;
#endif

        /// <summary>
        /// Can instances of this Type be <c>null</c>?
        /// </summary>
        public bool CanBeNull
        {
            get
            {
                if (type is null)
                    return true;
                if (type.IsStatic)
                    return false;
                if (type.IsValueType)
                    return Nullable.GetUnderlyingType(type) is not null;
                Debug.Assert(type.IsClass || type.IsInterface);
                return true;
            }
        }
        
        public bool IsNullable()
        {
            return type is not null && Nullable.GetUnderlyingType(type) is not null;
        }

        public bool IsNullable([NotNullWhen(true)] out Type? underlyingType)
        {
            if (type is not null)
            {
                underlyingType = Nullable.GetUnderlyingType(type);
                return underlyingType is not null;
            }

            underlyingType = null;
            return false;
        }


        /// <summary>
        /// Enumerate over all base types for this <see cref="Type"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> BaseTypes()
        {
            if (type is null)
                yield break;
            for (Type? baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
            {
                yield return baseType;
            }
        }

        /// <summary>
        /// Does this <see cref="Type"/> have the given <paramref name="genericTypeDefinition"/>?
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        public bool HasGenericTypeDefinition(Type? genericTypeDefinition)
        {
            if (type is null || !type.IsGenericType || genericTypeDefinition is null)
                return false;
            return type.GetGenericTypeDefinition() == genericTypeDefinition;
        }


        /// <summary>
        /// Does <c>this</c> <paramref name="type"/> implement the <paramref name="checkType"/>?
        /// </summary>
        /// <param name="checkType">The <see cref="Type"/> that this <paramref name="type"/> must implement</param>
        /// <returns>
        /// <c>true</c> if <paramref name="type"/> implements <paramref name="checkType"/>; otherwise, <c>false</c>
        /// </returns>
        public bool Implements(Type? checkType)
        {
            if (checkType is null)
                return false;
            if (type is null)
                return false;

            if (type == checkType)
                return true;
            // ref structs, byrefs, and pointers do not implement anything
            if (type.IsRef || type.IsPointer)
                return false;
            
            // Everything else implements object
            if (checkType == typeof(object))
                return true;

            if (!checkType.IsGenericTypeDefinition)
            {
                if (checkType.IsInterface)
                {
                    // Check my interfaces
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        if (interfaceType == checkType)
                            return true;
                    }

                    return false;
                }

                // Check my base types
                for (Type? baseType = type.BaseType;
                     baseType is not null /*&& baseType != typeof(object)*/;
                     baseType = baseType.BaseType)
                {
                    if (baseType == checkType)
                        return true;
                }
            }
            else
            {
                // Check direct (List<T> : List<>)
                if (type.HasGenericTypeDefinition(checkType))
                    return true;

                // Check my base types
                for (Type? baseType = type.BaseType;
                     baseType is not null /* && baseType != typeof(object)*/;
                     baseType = baseType.BaseType)
                {
                    if (baseType.HasGenericTypeDefinition(checkType))
                        return true;
                }

                // Check my interfaces
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType.HasGenericTypeDefinition(checkType))
                        return true;
                }
            }

            return false;
        }

        public bool Implements<T>()
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
#pragma warning disable CA2263
            => type.Implements(typeof(T));
        
        
        
        internal (T Min, T Max) GetMinMaxValues<T>()
        {
            if (type == typeof(sbyte))
            {
                return (Notsafe.As<sbyte, T>(sbyte.MinValue), Notsafe.As<sbyte, T>(sbyte.MaxValue));
            }
            else if (type == typeof(byte))
            {
                return (Notsafe.As<byte, T>(byte.MinValue), Notsafe.As<byte, T>(byte.MaxValue));
            }
            else if (type == typeof(short))
            {
                return (Notsafe.As<short, T>(short.MinValue), Notsafe.As<short, T>(short.MaxValue));
            }
            else if (type == typeof(ushort))
            {
                return (Notsafe.As<ushort, T>(ushort.MinValue), Notsafe.As<ushort, T>(ushort.MaxValue));
            }
            else if (type == typeof(int))
            {
                return (Notsafe.As<int, T>(int.MinValue), Notsafe.As<int, T>(int.MaxValue));
            }
            else if (type == typeof(uint))
            {
                return (Notsafe.As<uint, T>(uint.MinValue), Notsafe.As<uint, T>(uint.MaxValue));
            }
            else if (type == typeof(long))
            {
                return (Notsafe.As<long, T>(long.MinValue), Notsafe.As<long, T>(long.MaxValue));
            }
            else if (type == typeof(ulong))
            {
                return (Notsafe.As<ulong, T>(ulong.MinValue), Notsafe.As<ulong, T>(ulong.MaxValue));
            }
            else if (type == typeof(char))
            {
                return (Notsafe.As<char, T>(char.MinValue), Notsafe.As<char, T>(char.MaxValue));
            }
            else
            {
                throw Ex.Arg(type);
            }
        }
    }
}