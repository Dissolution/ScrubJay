using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ScrubJay.Enums.SourceGen.Models;

namespace ScrubJay.Enums.SourceGen.Utilities;

public static class SourceGenExtensions
{
    // determine the namespace the class/enum/struct is declared in, if any
    public static string GetNamespace(this BaseTypeDeclarationSyntax syntax)
    {
        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        string nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        SyntaxNode? potentialNamespaceParent = syntax.Parent;
    
        // Keep moving "out" of nested classes etc until we get to a namespace
        // or until we run out of parents
        while (potentialNamespaceParent != null &&
            potentialNamespaceParent is not NamespaceDeclarationSyntax
            && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();
        
            // Keep moving "out" of the namespace declarations until we 
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }

    
    public static Type? GetEnumUnderlyingType(this INamedTypeSymbol symbol) => symbol.EnumUnderlyingType?.SpecialType switch
    {
        SpecialType.System_SByte => typeof(sbyte),
        SpecialType.System_Byte => typeof(byte),
        SpecialType.System_Int16 => typeof(short),
        SpecialType.System_UInt16 => typeof(ushort),
        SpecialType.System_Int32 => typeof(int),
        SpecialType.System_UInt32 => typeof(uint),
        SpecialType.System_Int64 => typeof(long),
        SpecialType.System_UInt64 => typeof(ulong),
        _ => null,
    };

    public static SGArray<AttributeDefinition> GetAttributeDefinitions(this ISymbol symbol)
    {
        var attributes = symbol.GetAttributes();
        return ToAttributeDefinitions(attributes);
    }

    public static SGArray<AttributeDefinition> ToAttributeDefinitions(this in ImmutableArray<AttributeData> attributes)
    {
        if (attributes.IsDefault)
            return SGArray<AttributeDefinition>.Default;
        
        if (attributes.IsEmpty)
            return SGArray<AttributeDefinition>.Empty;

        var attributeDefinitions = new List<AttributeDefinition>(attributes.Length);
        
        foreach (AttributeData? attributeData in attributes)
        {
            if (AttributeDefinition.TryCreate(attributeData, out var def))
            {
                attributeDefinitions.Add(def);
            }
        }
        
        return SGArray.Create(attributeDefinitions);
    }
}