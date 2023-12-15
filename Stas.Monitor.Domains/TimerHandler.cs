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
}
