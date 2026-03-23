using System.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;

namespace ScrubJay.Text.Benchmarks.BDN;

[PublicAPI]
public sealed class MyOrderer : IOrderer
{
    public static readonly MyOrderer Instance = new();

    private static string? GetFirstParamValueAsString(BenchmarkCase benchmarkCase)
    {
        if (benchmarkCase.Parameters?.Count >= 1)
        {
            return benchmarkCase.Parameters[0].Value?.ToString();
        }
        return null;
    }

    private static double GetMeanNanoseconds(BenchmarkCase benchmarkCase, Summary summary)
    {
        BenchmarkReport? report = summary[benchmarkCase];
        return report?.ResultStatistics?.Mean ?? double.MaxValue;
    }

    public bool SeparateLogicalGroups => true;

    public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule>? rules = null) => benchmarksCase;

    public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary)
    {
        return benchmarksCases
            .OrderBy(static bc => bc.GetRuntime().Name, Comparers.NumericStringComparer)
            .ThenBy(static bc => bc.Parameters?.Count ?? 0)
            .ThenBy(static bc => GetFirstParamValueAsString(bc), Comparers.StringLengthOrdinalComparer)
            .ThenBy(bc => GetMeanNanoseconds(bc, summary));
    }

    public string? GetHighlightGroupKey(BenchmarkCase benchmarkCase) => GetFirstParamValueAsString(benchmarkCase);

    public string? GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) => GetFirstParamValueAsString(benchmarkCase);

    public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule>? rules = null)
        => logicalGroups.OrderBy(g => g.Key, StringLengthOrdinalComparer.Instance);


}