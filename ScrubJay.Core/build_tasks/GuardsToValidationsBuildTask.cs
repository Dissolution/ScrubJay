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
        bool needToEndNet9Block = false;

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            
            if (string.IsNullOrWhiteSpace(line))
            {
                if (needToEndNet9Block)
                {
                    builder.AppendLine("#endif");
                    inNet9Block = false;
                    needToEndNet9Block = false;
                }

                builder.AppendLine();
                continue;
            }

            if (line.EndsWith("}"))
            {
                builder.AppendLine(line);
                if (needToEndNet9Block)
                {
                    builder.AppendLine("#endif");
                    inNet9Block = false;
                    needToEndNet9Block = false;
                }

                continue;
            }

            if (line.StartsWith("#if NET9_0"))
            {
                inNet9Block = true;
            }
            else if (inNet9Block && line.StartsWith("#endif"))
            {
                inNet9Block = false;
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
                if (inNet9Block || m.Groups[1].Value.Contains("Span"))
                {
                    if (!inNet9Block)
                    {
                        builder.AppendLine("#if NET9_0_OR_GREATER");
                        needToEndNet9Block = true;
                    }

                    builder.Append(line, 0, m.Index)
                        .Append("public static RefResult<");
                }
                else
                {
                    builder.Append(line, 0, m.Index)
                        .Append("public static Result<");
                }

                builder.Append(m.Groups[1])
                    .Append("> ")
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

        if (needToEndNet9Block)
        {
            builder.AppendLine("#endif");
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