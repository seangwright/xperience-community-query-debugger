using Spectre.Console;

using SQL.Formatter;
using SQL.Formatter.Language;

using StackExchange.Profiling;

namespace XperienceCommunity.QueryDebugger;

public class ConsolePrintOptions
{
    public bool PopulateCache { get; set; } = true;
    public bool Verbose { get; set; } = false;
}

public static class MiniProfilerConsoleExtensions
{
    public static async Task PrintQuery(this MiniProfiler? profiler, string queryName, Func<Task> query, ConsolePrintOptions? options = null)
    {
        options ??= new();
        if (options.PopulateCache)
        {
            using var d = profiler.Ignore();
            await query();
        }

        using var t = profiler.Step(queryName);
        await query();
        t.Print(options);
    }

    public static void Print(this Timing? timing, ConsolePrintOptions options)
    {
        if (timing is null || !timing.HasCustomTimings)
        {
            return;
        }

        var root = new List<Padder>
        {
            new(new Text($"Query: {(timing.Name ?? "").ToUpperInvariant()}", new Style(Color.Green)))
        };
        foreach (string type in timing.CustomTimings.Keys)
        {
            foreach (var customTiming in timing.CustomTimings[type])
            {
                if (customTiming.CommandString is null
                    || customTiming.CommandString.StartsWith("Connection", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (options.Verbose)
                {
                    root.Add(new Padder(new Text($"Type: {customTiming.ExecuteType}", new Style(Color.Yellow))).PadLeft(2).PadBottom(0).PadTop(0));
                    root.Add(new Padder(new Text($"Stack Trace: {customTiming.StackTraceSnippet}", new Style(Color.Yellow))).PadLeft(2).PadBottom(0).PadTop(0));
                    root.Add(new Padder(new Text($"Start (ms): {customTiming.StartMilliseconds}", new Style(Color.Yellow))).PadLeft(2).PadBottom(0).PadTop(0));
                    root.Add(new Padder(new Text($"Duration (ms): {customTiming.DurationMilliseconds}", new Style(Color.Yellow))).PadLeft(2).PadBottom(0).PadTop(0));

                    if (customTiming.FirstFetchDurationMilliseconds.HasValue)
                    {
                        root.Add(new Padder(new Text($"First Fetch (ms): {customTiming.FirstFetchDurationMilliseconds}", new Style(Color.Yellow))).PadLeft(2).PadBottom(0).PadTop(0));
                    }
                }

                root.Add(new Padder(new Text(type.ToUpperInvariant(), new Style(Color.Yellow))).PadLeft(2).PadLeft(2).PadBottom(0).PadTop(0));
                string formatted = SqlFormatter
                    .Of(Dialect.TSql)
                    .Format(customTiming.CommandString);
                root.Add(new Padder(new Text(formatted)).PadLeft(4).PadBottom(2).PadTop(0));
            }
        }

        AnsiConsole.Write(new Rows(root));
    }
}
