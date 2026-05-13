using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using ScrubJay.Enums.SourceGen.Factories;
using ScrubJay.Enums.SourceGen.Models;
using ScrubJay.Enums.SourceGen.Utilities;
using SGF;

namespace ScrubJay.Enums.SourceGen;

[IncrementalGenerator]
public class EEG2 : IncrementalGenerator
{
    public EEG2() : base(nameof(EEG2))
    {
        var logger = new DebuggingLogger();
        Reflect.SetMember(this, g => g.Logger, logger);
    }

    public override void OnInitialize(SgfInitializationContext context)
    {
//        if (!Debugger.IsAttached)
//        {
//            Debugger.Launch();
//            Debugger.Break();
//        }

        // First pass: all referenced enums (I know)
        var enumNames = context
            .CompilationProvider
            .Select(ReferencedEnums);

        // they have been logged

        // generate an output file
        context.RegisterSourceOutput(enumNames, Output);
    }

    private void Output(SgfSourceProductionContext context, ImmutableHashSet<EnumToExtend> enumsToExtend)
    {
        //var code = string.Join("\n", enumsToExtend);
        //context.AddSource("enum_names.txt", SourceText.From(code, Encoding.UTF8));

        List<GeneratedCodeBuilder> factories = new();

        foreach (var e2e in enumsToExtend)
        {
            // Every enum
            factories.Add(new UniversalEnumExtensionsGeneratedCodeBuilder(e2e));
            
            // ...
        }

        foreach (var factory in factories)
        {
            var generatedCode = factory.Generate();
            context.AddGeneratedCode(generatedCode);
        }
    }

    private ImmutableHashSet<EnumToExtend> ReferencedEnums(Compilation compilation, CancellationToken cancellationToken = default)
    {
        var enums = new HashSet<string>(StringComparer.Ordinal);
        
        var enumsToExtend = new HashSet<EnumToExtend>();
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetRoot(cancellationToken);
            foreach (var node in root.DescendantNodes())
            {
                cancellationToken.ThrowIfCancellationRequested();
                var typeInfo = semanticModel.GetTypeInfo(node, cancellationToken);
                INamedTypeSymbol? enumSymbol = (typeInfo.Type ?? typeInfo.ConvertedType) as INamedTypeSymbol;
                if (enumSymbol is null || enumSymbol.TypeKind != TypeKind.Enum)
                    continue;
                // Collect the values we need for code generation
                var fullname = enumSymbol.ToDisplayString();
                if (!enums.Add(fullname))
                    continue;
                
                string enumTypeNameSpace = enumSymbol.ContainingNamespace.ToDisplayString();
                string enumTypeName = enumSymbol.Name;
                Type enumUnderlyingType = enumSymbol.GetEnumUnderlyingType()!;
                //bool enumIsUnsigned = Type.GetTypeCode(enumUnderlyingType) is TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64;
                SGArray<AttributeDefinition> attributes = enumSymbol.GetAttributes().ToAttributeDefinitions();

                // All of the Enum Member's Information
                ImmutableArray<ISymbol> enumMemberSymbols = enumSymbol.GetMembers();
                List<EnumMemberToExtend> membersToExtend = new(enumMemberSymbols.Length);

                foreach (ISymbol member in enumMemberSymbols)
                {
                    if (member is IFieldSymbol memberField && memberField.ConstantValue is not null)
                    {
                        // collect member values
                        string memberName = memberField.Name;
                        object? memberValue = memberField.ConstantValue;
                        var memberAttrs = memberField.GetAttributeDefinitions();

                        EnumMemberToExtend memberToExtend = new(memberName, memberValue, memberAttrs);
                        membersToExtend.Add(memberToExtend);
                    }
                }

                var e2e = new EnumToExtend(enumTypeNameSpace, enumTypeName, enumUnderlyingType, attributes, membersToExtend);
                enumsToExtend.Add(e2e);
            }
        }

        //Logger.Log(LogLevel.Information, null, $"Referenced Enum Names: {string.Join(", ", enums))}");

        return enumsToExtend.ToImmutableHashSet();
    }



    private bool IsEnumRef(SyntaxNode node, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private EnumInfo? Tx(GeneratorSyntaxContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public sealed record class EnumInfo
{
    public required string TypeNamespace { get; init; }
    public required string TypeName { get; init; }
    public required Type UnderlyingType { get; init; }
    public SGArray<AttributeDefinition> Attributes { get; init; } = SGArray<AttributeDefinition>.Empty;
    //public readonly SGArray<EnumMemberToExtend> Members;

    public string TypeFullName => $"{TypeNamespace}.{TypeName}";

    public bool HasFlagsAttribute
    {
        get
        {
            return Attributes.Any(attr => attr.TypeFullName == "System.FlagsAttribute");
        }
    }

    public bool IsUnsigned
    {
        get
        {
            return Type.GetTypeCode(UnderlyingType) is TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64;
        }
    }

    public EnumInfo()
    {
    }

}