//using System.Reflection;
//using System.Reflection.Emit;
//using ScrubJay.Functional;
//using ScrubJay.Text.Building;
//
//namespace ScrubJay.Sandboxes.Console;
//
//#if NET10_0_OR_GREATER
//
//public enum NullableAttributeContext : byte
//{
//    Oblivious = 0,
//    NonNullable = 1,
//    Nullable = 2,
//}
//
//[PublicAPI]
//public static partial class RuntimeBuilder
//{
//    public static PersistedAssemblyBuilder CreatePersistedAssemblyBuilder(string assemblyName)
//    {
//        return new PersistedAssemblyBuilder(new(assemblyName), typeof(object).Assembly);
//    }
//
//    public static ModuleBuilder CreateModuleBuilder(AssemblyBuilder assemblyBuilder, string? moduleName = null)
//    {
//        return assemblyBuilder.DefineDynamicModule(
//            moduleName ??
//            assemblyBuilder.FullName ?? 
//            assemblyBuilder.GetName().FullName);
//    }
//
//    public static CustomAttributeBuilder CreateCustomAttributeBuilder(Type attributeType, params object?[] constructorArgs)
//    {
//        Type[] argTypes = Array.ConvertAll(constructorArgs, obj => obj?.GetType() ?? typeof(object));
//        var ctor = attributeType.GetConstructor(argTypes);
//        if (ctor is null)
//            throw new InvalidOperationException();
//        return new CustomAttributeBuilder(ctor, constructorArgs);
//    }
//
//    public static CustomAttributeBuilder CreateNullableAttribute(NullableAttributeContext value)
//    {
//        return CreateCustomAttributeBuilder(typeof(System.Runtime.CompilerServices.NullableAttribute), [(byte)value]);
//    }
//    
//    public static CustomAttributeBuilder CreateNullableContextAttribute(byte value)
//    {
//        return CreateCustomAttributeBuilder(typeof(System.Runtime.CompilerServices.NullableContextAttribute), [(byte)value]);
//    }
//    
//    public static Result<string> TryCreateInterestingDelegate()
//    {
//        try
//        {
//            return CreateInterestingDelegate();
//        }
//        catch (Exception ex)
//        {
//            return ex;
//        }
//    }
//
//    private static string CreateInterestingDelegate()
//    {
//        const string NAMESPACE = "ScrubJay.Theories";
//        
//        var assembly = CreatePersistedAssemblyBuilder(NAMESPACE);
//        var module = CreateModuleBuilder(assembly, NAMESPACE);
//        
//        var delegateType = module.DefineType(
//            $"{NAMESPACE}.TextBuilderAction",
//            TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass,
//            typeof(MulticastDelegate));
//        delegateType.SetCustomAttribute(CreateNullableContextAttribute(1));
//        
//        var genericTypeParameters = delegateType.DefineGenericParameters("T");
//        var t = genericTypeParameters[0];
//        t.SetCustomAttribute(CreateNullableAttribute(NullableAttributeContext.Nullable));
//        
//        var ctor = delegateType.DefineConstructor(
//            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName,
//            CallingConventions.Standard,
//            [typeof(object), typeof(IntPtr)]
//        );
//        ctor.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
//        
//        var invoke = delegateType.DefineMethod(
//            "Invoke",
//            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
//            typeof(void),
//            [typeof(TextBuilder), t]
//        );
//        invoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
//        var invokeMethodTextBuilderParameter = invoke.DefineParameter(1, ParameterAttributes.None, "textBuilder");
//        var invokeMethodValueParameter = invoke.DefineParameter(2, ParameterAttributes.None, "value");
//        invokeMethodValueParameter.SetCustomAttribute(CreateNullableAttribute(NullableAttributeContext.Nullable));
//
//        var thingMethod = delegateType.DefineMethod("Thing",
//            MethodAttributes.Public | MethodAttributes.Static,
//            typeof(void),
//            Type.EmptyTypes);
//        var thingGen = thingMethod.GetILGenerator();
//        thingGen.EmitWriteLine("THING!!!");
//        thingGen.Emit(OpCodes.Ret);
//
//        var prop = delegateType.DefineProperty("Prop", PropertyAttributes.None, typeof(int), []);
//        var propGet = delegateType.DefineMethod(
//            "get_Prop",
//            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
//            typeof(int),
//            Type.EmptyTypes);
//        var propGetGen = propGet.GetILGenerator();
//        propGetGen.EmitWriteLine("PROP_GET!!!");
//        propGetGen.Emit(OpCodes.Ldc_I4, 147);
//        propGetGen.Emit(OpCodes.Ret);
//        prop.SetGetMethod(propGet);
//        
//        
//        delegateType.CreateType();
//        
//        // save to file
//        string fileName = @"c:\temp\ScrubJay.Theories.dll";
//        assembly.Save(fileName);
//        return fileName;
//    }
//}
//
//#endif