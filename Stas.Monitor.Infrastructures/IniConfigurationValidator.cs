using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures;

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

        var databaseValues = data["database"];

        var timers = new HashSet<int>();
        foreach (var timerString in data["timers"].Values)
        {
            timers.Add(int.Parse(timerString));
        }

        return new Configuration(thermometers, databaseValues, new TimerHandler(timers));
    }
}
