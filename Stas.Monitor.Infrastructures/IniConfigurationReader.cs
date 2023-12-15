using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

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
        try
        {
            var data = IniDataExtractor.ExtractData(_filePath);

            return IniConfigurationValidator.CheckArgumentsValidity(data);
        }
        catch (FileNotFoundException)
        {
            throw new ConfigurationReaderException("monitor: configuration file not found");
        }
    }
}
