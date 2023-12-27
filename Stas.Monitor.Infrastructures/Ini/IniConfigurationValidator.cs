using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures.Ini;

public static class IniConfigurationValidator
{
    public static Configuration CheckArgumentsValidity(IDictionary<string, IDictionary<string, string>> data)
    {
        var thermometers = VerifyThermometers(data);

        var measurementsDataPath = ParseMeasurementsDataPath(data);

        var timers = VerifyTimers(data);

        return new Configuration(thermometers, measurementsDataPath, new TimerHandler(timers));
    }

    private static ISet<string> VerifyThermometers(IDictionary<string, IDictionary<string, string>> data)
    {
        if (!data.ContainsKey("thermometers"))
        {
            throw new ConfigurationReaderException("monitor: missing required section thermometers");
        }

        var thermometerValues = data["thermometers"].Values;
        if (thermometerValues.Count == 0)
        {
            throw new ConfigurationReaderException("monitor: missing required section thermometers");
        }

        var thermometers = new HashSet<string>();
        foreach (var thermometerName in thermometerValues)
        {
            thermometers.Add(thermometerName);
        }

        return thermometers;
    }

    private static ISet<int> VerifyTimers(IDictionary<string, IDictionary<string, string>> data)
    {
        var timers = new HashSet<int>();
        if (!data.ContainsKey("timers"))
        {
            throw new ConfigurationReaderException("monitor: missing required section timers");
        }

        var timersValues = data["timers"].Values;
        if (timersValues.Count == 0)
        {
            throw new ConfigurationReaderException("monitor: missing required section timers");
        }

        foreach (var timerValue in timersValues)
        {
            timers.Add(int.Parse(timerValue));
        }

        return timers;
    }

    private static IDictionary<string, string> ParseMeasurementsDataPath(IDictionary<string, IDictionary<string, string>> data) //TODO utiliser et rajouter type dans map ?
    {
        if (data.TryGetValue("database", out var database))
        {
            return database;
        }

        if (data.TryGetValue("paths", out var path))
        {
            if (!path.ContainsKey("measurements"))
            {
                throw new ConfigurationReaderException("monitor: missing required properties measurements file path.");
            }

            if (!path.ContainsKey("alerts"))
            {
                throw new ConfigurationReaderException("monitor: missing required properties alerts file path.");
            }

            return path;
        }

        throw new ConfigurationReaderException("monitor: no configurations were set to retrieve data.");
    }
}
