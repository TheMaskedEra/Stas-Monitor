using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures.Ini;

public static class IniConfigurationValidator
{
    public static Configuration CheckArgumentsValidity(IDictionary<string, IDictionary<string, string>> data)
    {
        var thermometers = new HashSet<string>();
        if (!data.ContainsKey("thermometers"))
        {
            throw new ConfigurationReaderException("monitor: missing required section thermometers");
        }

        var thermometerValues = data["thermometers"].Values;
        foreach (var thermometer in thermometerValues)
        {
            thermometers.Add(thermometer);
        }

        if (thermometers.Count == 0)
        {
            throw new ConfigurationReaderException("monitor: missing required section thermometers");
        }

        var measurementsDataPath = ParseMeasurementsDataPath(data);

        var timers = new HashSet<int>();
        foreach (var timerString in data["timers"].Values)
        {
            timers.Add(int.Parse(timerString));
        }

        return new Configuration(thermometers, measurementsDataPath, new TimerHandler(timers));
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
                throw new ConfigurationReaderException("monitor: missing required properties alerts filepath.");
            }

            return path;
        }

        throw new ConfigurationReaderException("monitor: no configurations were set to retrieve data.");
    }
}
