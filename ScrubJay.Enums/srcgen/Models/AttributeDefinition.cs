using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using ScrubJay.Enums.SourceGen.Utilities;

namespace ScrubJay.Enums.SourceGen.Models;

[StructLayout(LayoutKind.Auto)]
public readonly record struct AttributeDefinition
{
    public static bool TryCreate(AttributeData? attributeData, out AttributeDefinition attributeDefinition)
    {
        if (attributeData is null || attributeData.AttributeClass is null)
        {
            attributeDefinition = default;
            return false;
        }

        attributeDefinition = new(
            attributeData.AttributeClass.ToDisplayString(),
            SGArray.Create(attributeData.ConstructorArguments, static arg => arg.Value),
            SGArray.Create(attributeData.NamedArguments, static pair => (pair.Key, pair.Value.Value))
        );
        return true;
    }

    public readonly string TypeFullName;
    public readonly SGArray<object?> ConstructorArguments;
    public readonly SGArray<(string ParamName, object? ParamValue)> NamedArguments;

    private AttributeDefinition(string typeFullName, SGArray<object?> constructorArguments, SGArray<(string ParamName, object? ParamValue)> namedArguments)
    {
        TypeFullName = typeFullName;
        ConstructorArguments = constructorArguments;
        NamedArguments = namedArguments;
    }
}