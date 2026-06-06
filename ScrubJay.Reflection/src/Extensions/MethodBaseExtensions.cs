using ScrubJay.Errors;

namespace ScrubJay.Reflection.Extensions;

/// <summary>
/// Extensions on <see cref="MethodBase"/>.
/// </summary>
[PublicAPI]
public static class MethodBaseExtensions
{
    extension(MethodBase? method)
    {
        /// <summary>
        /// Gets the <see cref="Visibility"/> of this <paramref name="method"/>.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                Visibility visibility = default;
                if (method is not null)
                {
                    visibility |= method.IsStatic ? Visibility.Static : Visibility.Instance;
                    if (method.IsPublic)
                        visibility |= Visibility.Public;
                    if (method.IsAssembly || method.IsFamilyAndAssembly || method.IsFamilyOrAssembly)
                        visibility |= Visibility.Internal;
                    if (method.IsFamily || method.IsFamilyAndAssembly || method.IsFamilyOrAssembly)
                        visibility |= Visibility.Protected;
                    if (method.IsPrivate)
                        visibility |= Visibility.Private;
                }

                return visibility;
            }
        }

        /// <summary>
        /// Can this <see cref="MethodBase"/> be overriden?
        /// </summary>
        /// <see href="https://stackoverflow.com/questions/38078948/check-if-a-classes-property-or-method-is-declared-as-sealed"/>
        public bool IsOverridable => method is { IsVirtual: true, IsFinal: false };

        /// <summary>
        /// Is this <see cref="MethodBase"/> <see langword="sealed"/>?
        /// </summary>
        public bool IsSealed => method is not null && (method.IsFinal || !method.IsVirtual);

        /// <summary>
        /// Is this <see cref="MethodBase"/> declared as <see langword="async"/>?
        /// </summary>
        public bool IsAsync
        {
            get
            {
                if (method is null)
                    return false;
                return typeof(IAsyncStateMachine).IsAssignableFrom(method.DeclaringType);
            }
        }


        /// <summary>
        /// Get the <see cref="Type">Types</see> of the parameters in this <see cref="MethodBase"/>
        /// </summary>
        public Type[] GetParameterTypes()
        {
            if (method is null) return [];
            return Array.ConvertAll(method.GetParameters(), static param => param.ParameterType);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> returned by this <see cref="MethodBase"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public Type ReturnType =>
            method switch
            {
                null => typeof(void),
                MethodInfo info => info.ReturnType,
                ConstructorInfo { IsStatic: true } => typeof(void),
                ConstructorInfo ctor => ctor.DeclaringType!,
                _ => throw Ex.ThisMethodIsNotSupported(method),
            };
        
        public Type[] GetGenericTypes()
        {
            if (method is null)
                return Type.EmptyTypes;
            return method.GetGenericArguments();
        }
    }


    //
    //
    //
    //
    // public static ParameterInfo ReturnParameter(this MethodBase method)
    // {
    //     if (method is MethodInfo methodInfo)
    //     {
    //         var parameter = methodInfo.ReturnParameter;
    //         return parameter ?? new ReturnParameterInfo(methodInfo, methodInfo.ReturnType);
    //     }
    //
    //     if (method is ConstructorInfo constructorInfo)
    //     {
    //         if (constructorInfo.IsStatic)
    //         {
    //             return new ReturnParameterInfo(constructorInfo, typeof(void));
    //         }
    //         else
    //         {
    //             return new ReturnParameterInfo(constructorInfo, constructorInfo.DeclaringType!);
    //         }
    //     }
    //
    //     throw new ArgumentException("Invalid Method", nameof(method));
    // }
    //
    // public static DecompiledILMethod Decompile(this MethodBase method) => DecompiledILMethod.Decompile(method);
}