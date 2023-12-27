namespace Stas.Monitor.Domains;

public class TimerHandler
{
    private readonly IDictionary<TimeSpan, string> _timers;

    public TimerHandler(IEnumerable<int> values)
    {
        _timers = new SortedDictionary<TimeSpan, string>();
        foreach (var value in values)
        {
            var timer = TimeSpan.FromSeconds(value);
            _timers.Add(timer, ConvertSecondsToString(timer));
        }
    }

    private static string ConvertSecondsToString(TimeSpan timer)
    {
        if (timer.TotalHours >= 1)
        {
            return $"{(int)timer.TotalHours}h {timer.Minutes}m {timer.Seconds}s";
        }

        return timer.TotalMinutes >= 1 ? $"{timer.Minutes}m {timer.Seconds}s" : $"{timer.Seconds}s";
    }

    public TimeSpan GetTimer(string value)
    {
        foreach (var pair in _timers)
        {
            if (pair.Value == value)
            {
                return pair.Key;
            }
        }

        return TimeSpan.Zero;
    }

    public IEnumerable<string> DisplayTimers => _timers.Values;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (TimerHandler)obj;
        var otherTimers = other.DisplayTimers.ToList();

        if (otherTimers.Count != DisplayTimers.Count())
        {
            return false;
        }

        foreach (var timer in DisplayTimers)
        {
            if (!otherTimers.Contains(timer))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => _timers.GetHashCode();
}
