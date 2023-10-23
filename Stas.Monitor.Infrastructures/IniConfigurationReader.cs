using System.Security.Principal;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.exceptions;

namespace Stas.Monitor.Infrastructures;

public class IniConfigurationReader : IConfigurationReader
{
  private readonly string _pathFile;
  public IniConfigurationReader(string fileName)
  {
    var path = Directory.GetCurrentDirectory() + "/" + fileName;
    if (!File.Exists(path) )
    {
      throw new FileNotFoundException("monitor: configuration file not found.");
    }

    _pathFile = path;
  }
  
  public Configuration ConfigureThermometers()
  {
    ISet<Thermometer> thermometers = new HashSet<Thermometer>();
    try
    {
      using (var reader = new StreamReader(_pathFile) )
      {
        string? line;
        while((line = reader.ReadLine()) != null)
        {
          if ( !line.StartsWith("[Thermometer") )
          {
            throw new UnableToExtractDataException("monitor: missing required section thermometers");
          }
          var supposedName = reader.ReadLine();
          var name = ParseName(supposedName);
          var supposedCsv = reader.ReadLine();
          var csv = ParseCsv(supposedCsv);

          var thermometer = new Thermometer(name);
          thermometers.Add(thermometer);
        }
      }
    }
    catch (IOException e)
    {
      throw new UnableToExtractDataException("monitor: something went wrong when extracting data.");
    }
    return new Configuration(thermometers);
  }
  private string ParseName(string? supposedName)
  {
    if ( supposedName == null || !supposedName.StartsWith("name=") )
    {
      throw new UnableToExtractDataException("monitor: missing required section thermometers");
    }

    var name = supposedName.Substring(supposedName.IndexOf('='));
    if ( string.IsNullOrEmpty(name) )
    {
      throw new UnableToExtractDataException("monitor: missing required section thermometers");
    }

    return name;
  }
  private string ParseCsv(string? supposedCsv)
  {
    if ( supposedCsv == null || !supposedCsv.StartsWith("name=") )
    {
      throw new UnableToExtractDataException("monitor: missing required section thermometers");
    }

    var csv = supposedCsv.Substring(supposedCsv.IndexOf('='));
    if ( string.IsNullOrEmpty(csv) )
    {
      throw new UnableToExtractDataException("monitor: missing required section thermometers");
    }

    return csv;
  }
}