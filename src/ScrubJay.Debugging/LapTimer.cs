using System.Collections.Concurrent;
using System.Resources;

namespace ScrubJay.Debugging;

public partial class LapTimer
{
    private static readonly Lock _lock = new Lock();
    private static readonly List<LapTimer> _nestedTimers = new();

    internal static void DeclareStopped(LapTimer timer)
    {
        lock (_lock)
        {
            if (_nestedTimers.Count == 0)
            {
                Debugger.Break();
                return;
            }

            var last = _nestedTimers[^1];
            if (timer != last)
            {
                Debugger.Break();
            }

            _nestedTimers.Remove(timer);
        }
    }

    public static LapTimer Run(string? name = null)
    {
        lock (_lock)
        {
            byte id = (byte)_nestedTimers.Count;
            var parent = id == 0 ? null : _nestedTimers[^1];
            var timer = new LapTimer(id, name, parent);
            timer.Start();
            _nestedTimers.Add(timer);
            parent?.AddChild(timer);
            return timer;
        }
    }
}

[MustDisposeResource(false)]
public partial class LapTimer :
#if NET7_0_OR_GREATER
    IEqualityOperators<LapTimer, LapTimer, bool>,
#endif
    IEquatable<LapTimer>,
    IDisposable
{
    public static bool operator ==(LapTimer? left, LapTimer? right)
    {
        if (left is not null)
        {
            return left.Equals(right);
        }
        else if (right is not null)
        {
            return right.Equals(left);
        }
        else
        {
            return true;
        }
    }

    public static bool operator !=(LapTimer? left, LapTimer? right)
    {
        if (left is not null)
        {
            return !left.Equals(right);
        }
        else if (right is not null)
        {
            return !right.Equals(left);
        }
        else
        {
            return false;
        }
    }

    private readonly byte _id;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly LapTimer? _parentTimer;
    private readonly List<LapTimer> _childTimers = new(capacity: 0);

    public string? Name { get; }

    public TimeSpan Elapsed => _stopwatch.Elapsed;

    internal LapTimer(byte id, string? name, LapTimer? parentTimer)
    {
        _id = id;
        _parentTimer = parentTimer;
        Name = name;
    }

    ~LapTimer()
    {
        this.Dispose();
    }

    internal void AddChild(LapTimer timer)
    {
        _childTimers.Add(timer);
    }

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Reset()
    {
        _stopwatch.Reset();
    }

    public void Restart()
    {
        _stopwatch.Restart();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        // force dispose all children
        _childTimers.ForEach(static timer => timer.Dispose());
        // dispose myself
        _stopwatch.Stop();
        // tell the handler I'm disposed
        LapTimer.DeclareStopped(this);
        GC.SuppressFinalize(this);
    }

    public bool Equals(LapTimer? other)
    {
        return other is not null && other._id == this._id;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is LapTimer lapTimer && Equals(lapTimer);

    public override int GetHashCode()
    {
        return _id;
    }

    internal void WriteTo(ref DefaultInterpolatedStringHandler interpolated, int indent = 0)
    {
        if (indent > 0)
        {
            interpolated.AppendLiteral(new string(' ', indent));
        }
        if (!string.IsNullOrEmpty(this.Name))
        {
            interpolated.AppendLiteral(this.Name!);
            interpolated.AppendLiteral(": ");
        }
        if (_stopwatch.IsRunning)
        {
            interpolated.AppendLiteral("Running for ");
        }
        else
        {
            interpolated.AppendLiteral("Stopped after ");
        }
        interpolated.AppendFormatted(_stopwatch.Elapsed, "HH:mm:ss.f");

        if (_childTimers.Count > 0)
        {
            interpolated.AppendLiteral(Environment.NewLine);
            if (indent > 0)
            {
                interpolated.AppendLiteral(new string(' ', indent));
            }
            interpolated.AppendLiteral("- ");
            interpolated.AppendFormatted(_childTimers.Count);
            interpolated.AppendLiteral(" sub-timers:");

            indent += 2; // two spaces per indent
            foreach (var timer in _childTimers)
            {
                interpolated.AppendLiteral(Environment.NewLine);
                timer.WriteTo(ref interpolated, indent);
            }
        }
    }

    public override string ToString()
    {
        // Show us + downstream
        DefaultInterpolatedStringHandler interpolated = new(512, _childTimers.Count + 2);
        WriteTo(ref interpolated);
        return interpolated.ToStringAndClear();
    }
}