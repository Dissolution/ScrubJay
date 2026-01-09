namespace ScrubJay.Universal;

partial class TypeName
{
    private static void WriteNestingInformation(StringBuilder builder, Type type, GenericTypes providedGenericTypes)
    {
        Debug.Assert(type.IsNested);

        if (!providedGenericTypes.IsFilled)
        {
            providedGenericTypes.Fill(type.GetGenericArguments());
        }
      
        var declaringType = type.DeclaringType ?? type.ReflectedType ?? type.Module.GetType();
        WriteType(builder, declaringType, providedGenericTypes);
        builder.Append('.');
    }
}

/// <summary>
/// Gets nicer names for <see cref="Type">Types</see>!<br/>
/// <code>
/// `System.Int32` -> `int`
/// `System.Collections.Generic.IList`1` -> `IList&lt;int&gt;`
///
/// </code>
/// </summary>
[PublicAPI]
public static partial class TypeName
{
    extension(StringBuilder builder)
    {
        public StringBuilder AppendTypeName(Type? type)
        {
            return builder.Append(For(type));
        }

        public StringBuilder AppendTypeName<T>()
        {
            return builder.Append(For<T>());
        }
    }

    private sealed class GenericTypes
    {
        public static GenericTypes Empty { get; } = new();
        
        private Type[]? _genericTypes;
        private int _offset;

        public bool IsFilled => _genericTypes is not null;

        public int Count => _genericTypes is null ? 0 : _genericTypes.Length - _offset;
        public Type this[int index] => _genericTypes[_offset + index];
        
        public GenericTypes()
        {
            _genericTypes = null;
            _offset = -1;
        }

        public void Fill(params Type[] genericTypes)
        {
            _genericTypes = genericTypes;
            _offset = 0;
        }

        public bool TryGet([NotNullWhen(true)] out Type? type)
        {
            if (_genericTypes is null)
            {
                type = null;
                return false;
            }
            
            if (_offset >= _genericTypes.Length)
            {
                type = null;
                return false;
            }

            type = _genericTypes[_offset];
            _offset++;
            return true;
        }

        public ReadOnlySpan<Type> FillTake(Type[] genericTypes)
        {
            if (_genericTypes is null)
            {
                _genericTypes = genericTypes;
                _offset = genericTypes.Length;
                return genericTypes;
            }
            else if (_genericTypes.SequenceEqual(genericTypes))
            {
                // we've already used some
                ReadOnlySpan<Type> types = _genericTypes.AsSpan(_offset);
                _offset = genericTypes.Length;
                return types;
            }
            else
            {
                int count = genericTypes.Length;
                if (_offset + count > _genericTypes.Length)
                {
                    Debugger.Break();
                }
                ReadOnlySpan<Type> types = _genericTypes.AsSpan(_offset, count);
                _offset += count;
                return types;
            }
        }
    }

    private static void WriteType(StringBuilder builder, Type? type, GenericTypes providedGenericTypes)
    {
        if (type is null)
        {
            builder.Append("null");
            return;
        }

        if (_typeAliases.TryGetValue(type, out var alias))
        {
            builder.Append(alias);
            return;
        }

        if (type.IsPointer)
        {
            type = type.GetElementType()!;
            WriteType(builder, type, providedGenericTypes);
            builder.Append('*');
            return;
        }

        if (type.IsByRef)
        {
            type = type.GetElementType()!;
            WriteType(builder, type, providedGenericTypes);
            builder.Append('&');
            return;
        }

        if (type.IsArray)
        {
            WriteArrayType(builder, type, providedGenericTypes);
            return;
        }

        if (type.IsNested && !type.IsGenericParameter)
        {
            WriteNestingInformation(builder, type, providedGenericTypes);
            // do not return
        }
        
        if (type.IsGenericType)
        {
            ReadOnlySpan<Type> genericTypes = providedGenericTypes.FillTake(type.GetGenericArguments()); 
          
            var genericTypeDef = type.GetGenericTypeDefinition();
            
            if (_tupleTypes.Contains(genericTypeDef))
            {
                WriteTupleType(builder, type, providedGenericTypes);
                return;
            }

            if (genericTypeDef == typeof(Nullable<>))
            {
                Debug.Assert(genericTypes.Length == 1);
                WriteType(builder, genericTypes[0], providedGenericTypes);
                builder.Append('?');
                return;
            }

            var name = type.Name;
            int i = name.LastIndexOf('`');
            if (i >= 0)
            {
                builder.Append(name, 0, i);
            }
            else
            {
                builder.Append(name);
            }

            builder.Append('<');
            WriteType(builder, genericTypes[0], providedGenericTypes);
            for (i = 1; i < genericTypes.Length; i++)
            {
                builder.Append(", ");
                WriteType(builder, genericTypes[i], providedGenericTypes);
            }
            builder.Append('>');
            return;
        }
        
        
        // just write the name
        builder.Append(type.Name);
    }
   
    /*
    public static string For(Type? type)
    {
        var builder = new StringBuilder();
        WriteType(builder, type, new());
        return builder.ToString();
    }
    
    public static string For<T>() => For(typeof(T));
    */
    public static string For(Type? type)
    {
        if (type is null)
            return string.Empty;
        
        var builder = new StringBuilder();
        TypeName.AppendTypeName(builder, type);
        return builder.ToString();
    }
    
    public static string For<T>() => For(typeof(T));
    
}