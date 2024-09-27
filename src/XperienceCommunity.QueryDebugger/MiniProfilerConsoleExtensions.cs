using System;
using System.Text;
using StackExchange.Profiling;

namespace XperienceCommunity.QueryDebugger;

public static class MiniProfilerConsoleExtensions
{
    public static async Task PrintQuery(this MiniProfiler? profiler, string queryName, Func<Task> query, bool isolateCache = true)
    {
        if (isolateCache)
        {
            using var d = profiler.Ignore();
            await query();
        }

        using var t = profiler.Step(queryName);
        await query();
        Console.WriteLine(t.BuildTimingString());
    }

    public static StringBuilder BuildTimingString(this Timing? timing, StringBuilder? sb = null)
    {
        sb ??= new StringBuilder();

        if (timing is null || !timing.HasCustomTimings)
        {
            return sb;
        }
        sb.Append(timing.Name).AppendLine(":");
        foreach (var type in timing.CustomTimings.Keys)
        {
            sb.Append("  ").Append(type).Append(':').AppendLine();
            foreach (var customTiming in timing.CustomTimings[type])
            {
                if (customTiming.CommandString is null)
                {
                    continue;
                }
                sb.Append("    Type:").AppendLine(customTiming.ExecuteType);
                sb.Append("    Stack Trace:").AppendLine(customTiming.StackTraceSnippet);
                sb.Append("    Start (ms):").AppendLine(customTiming.StartMilliseconds.ToString());
                sb.Append("    Duration (ms):").AppendLine(customTiming.DurationMilliseconds.ToString());
                if (customTiming.FirstFetchDurationMilliseconds.HasValue)
                {
                    sb.Append("    First Fetch (ms):").AppendLine(customTiming.FirstFetchDurationMilliseconds.ToString());
                }
                sb.Append("    Command:").AppendLine(customTiming.CommandString);
            }
        }

        return sb;
    }
}
