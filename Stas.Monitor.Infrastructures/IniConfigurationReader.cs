using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Infrastructures;

public class IniConfigurationReader : IConfigurationReader
{
    private readonly string _filePath;

    public IniConfigurationReader(string filePath)
    {
        _filePath = filePath;
    }

    public Configuration RetrieveConfiguration()
    {
        var dataExtractor = new IniDataExtractor();
        var data = dataExtractor.ExtractData(_filePath);

        var thermometers = new HashSet<string>();
        var thermometerValues = data["thermometers"].Values;
        foreach (var thermometer in thermometerValues)
        {
          thermometers.Add(thermometer);
        }

        var databaseValues = data["database"];

        return new Configuration(thermometers, databaseValues);
    }
}
