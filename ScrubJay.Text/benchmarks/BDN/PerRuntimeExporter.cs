using System.Collections.Immutable;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using ScrubJay.Universal;

namespace ScrubJay.Text.Benchmarks.BDN;

[PublicAPI]
public sealed class PerRuntimeExporter : IExporter
{
    private readonly ImmutableArray<IExporter> _innerExporters;

    public string Name => $"PerRuntimeExporter_{_innerExporters.Length}";

    public PerRuntimeExporter(params IExporter[] innerExporters)
    {
        _innerExporters = innerExporters.ToImmutableArray();
    }

    public void ExportToLog(Summary summary, ILogger logger)
    {
        foreach (var exporter in _innerExporters)
        {
            exporter.ExportToLog(summary, logger);
        }
    }

    public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
    {
        Console.WriteLine($"PerRuntimeExporter processing Summary '{summary.Title}' with consolelogger '{consoleLogger.GetType().Render()}'");

        var files = new List<string>();

        // Group benchmark cases by their runtime moniker
        var benchmarkCasesByRuntime = summary
            .BenchmarksCases
            .GroupBy(static bc => bc.Job.Environment.Runtime?.MsBuildMoniker, Comparers.StringLengthOrdinalComparer);

        foreach (var group in benchmarkCasesByRuntime)
        {
            string? runtimeMoniker = group.Key;

            Console.WriteLine($"PerRuntimeExporter processing Runtime '{runtimeMoniker}'");

            // Build a filtered summary containing only this runtime's reports
            var filteredReports = summary
                .Reports
                .Where(report => report.BenchmarkCase.Job.Environment.Runtime?.MsBuildMoniker == runtimeMoniker)
                .ToImmutableArray();

            // new output path
            string monikerPath = (runtimeMoniker ?? "unknown").Replace('.', '_');
            
            var newResultsDirectory = Path.Combine(summary.ResultsDirectoryPath, monikerPath);
            Directory.CreateDirectory(newResultsDirectory);

            var filteredSummary = new Summary(
                title: $"{summary.Title}-{runtimeMoniker}",
                reports: filteredReports,
                hostEnvironmentInfo: summary.HostEnvironmentInfo,
                resultsDirectoryPath: newResultsDirectory,
                logFilePath: summary.LogFilePath,
                totalTime: summary.TotalTime,
                cultureInfo: summary.GetCultureInfo(),
                validationErrors: summary.ValidationErrors,
                columnHidingRules: summary.ColumnHidingRules,
                summaryStyle: summary.Style
            );

            foreach (var exporter in _innerExporters)
            {
                var exportedFiles = exporter.ExportToFiles(filteredSummary, consoleLogger);
                foreach (var filePath in exportedFiles)
                {
                    Console.WriteLine($"Exported to '{filePath}'");
                    files.Add(filePath);
                }
            }
        }

        return files;
    }


}