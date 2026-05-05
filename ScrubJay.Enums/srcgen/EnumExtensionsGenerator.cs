//using System.Collections.Immutable;
//using System.Diagnostics;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using ScrubJay.Enums.SourceGen.Factories;
//using ScrubJay.Enums.SourceGen.Models;
//using ScrubJay.Enums.SourceGen.Utilities;
//using SGF;
//
//namespace ScrubJay.Enums.SourceGen;
//
//[IncrementalGenerator]
//public class EnumExtensionsGenerator : IncrementalGenerator
//{
//    public EnumExtensionsGenerator() : base(nameof(EnumExtensionsGenerator))
//    {
//        var logger = new DebuggingLogger();
//        Reflect.SetMember(this, g => g.Logger, logger);
//    }
//
//    private bool ShouldTransformNode(SyntaxNode node, CancellationToken token)
//    {
//        // since we have the Attribute filter, we don't have much more to filter
//        if (node is EnumDeclarationSyntax)
//            return true;
//
//        Logger.Warning($"SyntaxNode '{node}' was not an EnumDeclarationSyntax");
//        Debugger.Break();
//        return false;
//    }
//
//    private EnumToExtend? CreateEnumToExtend(SemanticModel semanticModel, EnumDeclarationSyntax enumDeclarationSyntax, CancellationToken token)
//    {
//        INamedTypeSymbol? enumSymbol = semanticModel.GetDeclaredSymbol(enumDeclarationSyntax);
//        if (enumSymbol is null)
//        {
//            Logger.Error("Something went wrong");
//            Debugger.Break();
//            return null;
//        }
//
//        // Collect the values we need for code generation
//        string enumTypeNameSpace = enumDeclarationSyntax.GetNamespace();
//        string enumTypeName = enumSymbol.Name;
//        Type enumUnderlyingType = enumSymbol.GetEnumUnderlyingType()!;
//        bool enumIsUnsigned = Type.GetTypeCode(enumUnderlyingType) is TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64;
//        SGArray<AttributeDefinition> attributes = enumSymbol.GetAttributes().ToAttributeDefinitions();
//
//        // All of the Enum Member's Information
//        ImmutableArray<ISymbol> enumMemberSymbols = enumSymbol.GetMembers();
//        List<EnumMemberToExtend> membersToExtend = new(enumMemberSymbols.Length);
//
//        foreach (ISymbol member in enumMemberSymbols)
//        {
//            if (member is IFieldSymbol memberField && memberField.ConstantValue is not null)
//            {
//                // collect member values
//                string memberName = memberField.Name;
//                object? memberValue = memberField.ConstantValue;
//                var memberAttrs = memberField.GetAttributeDefinitions();
//
//                EnumMemberToExtend memberToExtend = new(memberName, memberValue, memberAttrs);
//                membersToExtend.Add(memberToExtend);
//            }
//        }
//
//        return new EnumToExtend(enumTypeNameSpace, enumTypeName, enumUnderlyingType, attributes, membersToExtend);
//    }
//
//    private void Execute(EnumToExtend? enumToExtend, SgfSourceProductionContext context)
//    {
//        if (!enumToExtend.HasValue)
//            return;
//
//        var e2e = enumToExtend.GetValueOrDefault();
//
//        List<GeneratedCodeBuilder> factories = new();
//
//        // Every enum
//        factories.Add(new UniversalEnumExtensionsGeneratedCodeBuilder(e2e));
//
//        // ...
//
//        foreach (var factory in factories)
//        {
//            var generatedCode = factory.Generate();
//            context.AddGeneratedCode(generatedCode);
//        }
//    }
//
//#if DEBUG
//    private static Mutex _mutex = new Mutex(false, "DebugMutext");
//
//    private static bool GateOneDebugger()
//    {
//        if (_mutex.WaitOne(0))
//        {
//            if (Debugger.IsAttached)
//                return false; // someone else is already attached
//            Debugger.Launch();
//            _mutex.ReleaseMutex();
//            return true;
//        }
//        return false;
//    }
//
//#endif
//
//    public override void OnInitialize(SgfInitializationContext context)
//    {
////#if DEBUG
////        //if (!GateOneDebugger())
////        //    return;
////
////        if (!Debugger.IsAttached)
////        {
////            Debugger.Launch();
////        }
////#endif
//
//        // Add our Enum
//        context.RegisterPostInitializationOutput(ctx =>
//        {
//            ctx.AddEmbeddedAttributeDefinition();
//            ctx.AddGeneratedCode(CommonCode.ExtendAttribute.GeneratedCode);
//        });
//
//        // First filter: Enums that have the Extends attribute
//        IncrementalValuesProvider<EnumToExtend?> enumsToExtend = context
//            .SyntaxProvider
//            .ForAttributeWithMetadataName(
//                "ScrubJay.Enums.ExtendAttribute",
//                predicate: (syntaxNode, token) => ShouldTransformNode(syntaxNode, token),
//                transform: (syntaxContext, token) => CreateEnumToExtend(syntaxContext.SemanticModel, (EnumDeclarationSyntax)syntaxContext.TargetNode, token))
//            .Where(static e2e => e2e is not null);
//
//        // Generate source code for each enum found
//        context.RegisterSourceOutput(enumsToExtend, (spc, e2e) => Execute(e2e, spc));
//    }
//
//    public override void OnException(Exception exception)
//    {
//        base.OnException(exception);
//    }
//}