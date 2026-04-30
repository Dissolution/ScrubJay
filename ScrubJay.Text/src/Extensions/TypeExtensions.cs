namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class TypeExtensions
{
    extension(Type)
    {
        public static string Render(Type type) => Renderer.RenderValue<Type>(type);

        public static string Render<T>() => Renderer.RenderValue<Type>(typeof(T));

        public static string Render<I>(in I? instance) => Renderer.RenderValue<Type>(Any.GetType(in instance));

#if NET9_0_OR_GREATER
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static string Render<I>(in I? instance, TypeConstraints.AllowsRefStruct<I> _ = default)
            where I : allows ref struct
            => Renderer.RenderValue<Type>(Any.GetType(in instance, _));
#endif
    }
    
    extension(Type? type)
    {
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(type))]
            get
            {
                if (type is not null)
                {
                    if (type.DeclaringType is not null)
                        return type.DeclaringType;
                    if (type.ReflectedType is not null)
                        return type.ReflectedType;
                    return type.Module.GetType();
                }
                return null;
            }
        }

        public bool IsTuple
        {
            get
            {
                if (type is not null)
                {
                    if (type.Namespace == "System" && (type.Name.StartsWith("Tuple") || type.Name.StartsWith("ValueTuple")))
                        return true;
                }
                return false;
            }
        }
        
        
        public bool ImplementsInterface(Type? interfaceType)
        {
            if (interfaceType is null || !interfaceType.IsInterface)
                return false;
            
            Type? baseType = type;
            while (baseType is not null)
            {
                Type[] interfaces = baseType.GetInterfaces();
                Debug.Assert(interfaces is not null);
                Debug.Assert(interfaces.All(i => i is not null));

                foreach (var it in interfaces!)
                {
                    if (it == interfaceType || it.ImplementsInterface(interfaceType))
                        return true;
                }
            
                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}