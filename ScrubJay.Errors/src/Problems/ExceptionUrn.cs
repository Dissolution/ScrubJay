using System.Reflection;
using System.Text.RegularExpressions;
using ScrubJay.Text.Collections;
using ScrubJay.Text.Extensions;
using Any = ScrubJay.Universal.Any;

namespace ScrubJay.Errors.Problems;

[PublicAPI]
public static class ExceptionUrn
{
    private static readonly TypeSet _exceptionTypes =
        AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(static assembly => !assembly.IsDynamic)
            .SelectMany(static assembly =>
            {
                try
                {
                    // only public types
                    return assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException typeLoadException)
                {
                    return typeLoadException.Types.WhereNotNull();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .Where(static type => type.IsAssignableTo(typeof(Exception)))
            .ToTypeSet();


    public static string ToTypeUrn<E>(E? exception)
        where E : Exception
    {
        var exceptionType = Any.GetType<E>(in exception);
        return ToTypeUrn(exceptionType);
    }

    [return: NotNullIfNotNull(nameof(exceptionType))]
    public static string? ToTypeUrn(Type? exceptionType)
    {
        if (exceptionType is not null)
        {
            return R($"urn:{exceptionType.Namespace}:{exceptionType.Name}");
        }
        return null;
    }

    public static Type? FromTypeUrn(string? urn)
    {
        if (string.IsNullOrEmpty(urn))
            return null;

        Regex regex = new Regex("urn:([a-zA-Z_][a-zA-Z0-9_.]*):([a-zA-Z_][a-zA-Z0-9_]+)", RegexOptions.Compiled);
        var match = regex.Match(urn);
        if (match.Success)
        {
            var ns = match.Groups[1].Value;
            var name = match.Groups[2].Value;
            return _exceptionTypes
                .FirstOrDefault(ex => ex.Namespace == ns && ex.Name == name);
        }

        foreach (var exType in _exceptionTypes)
        {
            if (string.Equals(urn, exType.Name, StringComparison.Ordinal))
                return exType;
            if (string.Equals(urn, exType.FullName, StringComparison.Ordinal))
                return exType;
        }

        return null; // no match
    }
}