using System.Collections.Immutable;
using System.Globalization;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Validators;
// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace ScrubJay.Text.Benchmarks.BDN;

internal static class BenchmarkConfigs
{
    // https://benchmarkdotnet.org/articles/configs/configs.html   

    private static Job[] GetRuntimedJobs(Func<Job> getBaseJob)
    {
        Runtime[] runtimes =
        [
            CoreRuntime.Core10_0,
            CoreRuntime.Core90,
            CoreRuntime.Core80,
            CoreRuntime.Core70,
            CoreRuntime.Core60,
            ClrRuntime.Net48,
        ];

        int count = runtimes.Length;

        Job[] jobs = new Job[count];
        for (var i = 0; i < count; i++)
        {
            var runtime = runtimes[i];
            jobs[i] = getBaseJob().WithId(runtime.MsBuildMoniker).WithRuntime(runtime);
        }
        return jobs;
    }


    public static ManualConfig ShortRun { get; } = new ManualConfig()
        .WithArtifactsPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\benchmark_results"))
        .WithOptions(ConfigOptions.StopOnFirstError | ConfigOptions.DontOverwriteResults)
        .AddColumnProvider(DefaultColumnProviders.Instance)
        .AddJob(GetRuntimedJobs(() => Job.ShortRun))
        .AddLogger(ConsoleLogger.Unicode)
        .AddExporter(new PerRuntimeExporter(HtmlExporter.Default, JsonExporter.Brief, MarkdownExporter.GitHub, CsvExporter.Default))
        .AddDiagnoser(MemoryDiagnoser.Default)
        .AddAnalyser(
            EnvironmentAnalyser.Default,
            OutliersAnalyser.Default,
            MinIterationTimeAnalyser.Default,
            MultimodalDistributionAnalyzer.Default,
            RuntimeErrorAnalyser.Default,
            ZeroMeasurementAnalyser.Default,
            BaselineCustomAnalyzer.Default,
            HideColumnsAnalyser.Default)
        .AddValidator(
            BaselineValidator.FailOnError,
            SetupCleanupValidator.FailOnError,
            JitOptimizationsValidator.FailOnError,
            RunModeValidator.FailOnError,
            GenericBenchmarksValidator.DontFailOnError,
            DeferredExecutionValidator.FailOnError,
            ParamsAllValuesValidator.FailOnError,
            ParamsValidator.FailOnError,
            RuntimeValidator.DontFailOnError)
        .WithSummaryStyle(SummaryStyle.Default)
        .WithOrderer(MyOrderer.Instance);
}