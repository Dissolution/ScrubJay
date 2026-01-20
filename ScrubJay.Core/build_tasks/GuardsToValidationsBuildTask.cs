using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace BuildTasks;

public sealed class GenerateValidateFilesTask : Task
{
    private static readonly char[] NEWLINE_CHARS = Environment.NewLine.ToCharArray();

    private static string GetDestFilePath(string sourceFilePath)
    {
        var dir = Path.GetDirectoryName(sourceFilePath);
        var fileName = Path
            .GetFileName(sourceFilePath)
            .Replace("Guard", "Validate")
            .Replace(".cs", ".g.cs");
        return Path.Combine(dir!, fileName);
    }
    
    private static string GetTransformedContent(string sourceContent)
    {
        var builder = new StringBuilder()
            .AppendLine("#nullable enable")
            .AppendLine();

        var lines = sourceContent.Split(NEWLINE_CHARS, StringSplitOptions.None);
        bool inNet9Block = false;
        
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var start = line.AsSpan().TrimStart();
            
            if (start.StartsWith("//#if NET9_0_OR_GREATER") || start.StartsWith("#if NET9_0_OR_GREATER"))
            {
                builder.AppendLine("#if NET9_0_OR_GREATER");
                inNet9Block = true;
                continue;
            }
            else if (start.StartsWith("//#endif") || start.StartsWith("#endif"))
            {
                builder.AppendLine("#endif");
                inNet9Block = false;
                continue;
            }

            line = line
                .Replace("Guard", "Validate")
                .Replace("throw ", "return ")
                .Replace(", NotNull]", "]");

            var m = Regex.Match(
                line,
                @"public\s+static\s+(\([^)]+\)|[^(\s]+(?:<[^>]+>)?)\s+(\w+)(<[^>]+>)?\s*\((.*)$",
                RegexOptions.Compiled);

            if (m.Success && m.Groups.Count >= 5)
            {
                var returnType = m.Groups[1].Value;
                if (returnType.Contains("Span") || inNet9Block)
                {
                    builder.Append(line, 0, m.Index)
                        .Append("public static RefResult<")
                        .Append(m.Groups[1])
                        .Append("> ");
                }
                else if (returnType == "bool")
                {
                    builder.Append(line, 0, m.Index)
                        .Append("public static Result ");
                }
                else
                {
                    builder.Append(line, 0, m.Index)
                        .Append("public static Result<")
                        .Append(m.Groups[1])
                        .Append("> ");
                }

                builder
                    .Append(m.Groups[2])
                    .Append(m.Groups[3])
                    .Append('(')
                    .Append(m.Groups[4]);
            }
            else
            {
                builder.Append(line);
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    [Required]
    public ITaskItem[] SourceFiles { get; set; } = [];

    public override bool Execute()
    {
        foreach (var file in SourceFiles)
        {
            try
            {
                string? sourcePath = file.GetMetadata("FullPath");
                if (sourcePath is null)
                    continue;
                string content = File.ReadAllText(sourcePath);

                string destPath = GetDestFilePath(sourcePath);
                string destContent = GetTransformedContent(content);

                File.WriteAllText(destPath, destContent, Encoding.UTF8);
                Log.LogMessage(MessageImportance.Normal, $"Generated {destPath}");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error processing {file.ItemSpec}: {ex.Message}");
                return false;
            }
        }

        return true;
    }
}