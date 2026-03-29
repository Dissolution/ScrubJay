#pragma warning disable MA0076

using System.Reflection;
using ScrubJay.Text.Benchmarks;
using ScrubJay.Text.Benchmarks.BDN;
using ScrubJay.Text.Benchmarks.CopyTo;

Console.OutputEncoding = Encoding.Unicode;

Stopwatch timer = Stopwatch.StartNew();

Console.WriteLine($"Starting at {DateTime.Now:G}");
var summary = BenchmarkRunner.Run<StringCopyToCharArrayBenchmarks>(BenchmarkConfigs.ShortRun, args);
timer.Stop();
Console.WriteLine($"Finished at {DateTime.Now:G} in {timer.Elapsed:g}");

Process.Start(new ProcessStartInfo(summary.ResultsDirectoryPath)
{
    UseShellExecute = true,
});
return;
