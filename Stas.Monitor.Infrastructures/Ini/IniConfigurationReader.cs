using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures.Ini;

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
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            throw new ConfigurationReaderException("monitor: configuration file not found");
        }
    }
}
