namespace Stas.Monitor.Presentations;

public class TimeSpanFormatter
{
    //TODO correcte ?
    public static string ToString(TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds < 60)
        {
            return $"{(int)timeSpan.TotalSeconds} seconds";
        }

        if (timeSpan.TotalMinutes < 60)
        {
            return$"{(int)timeSpan.TotalMinutes} minutes";
        }

        var hours = (int)timeSpan.TotalHours;
        var minutes = timeSpan.Minutes;
        return $"{hours} {(hours == 1 ? "hour" : "hours")} {minutes} {(minutes == 1 ? "minute" : "minutes")}";
    }
}
