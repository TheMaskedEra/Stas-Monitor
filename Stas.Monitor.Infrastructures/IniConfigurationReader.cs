using System.Collections;
using System.Text.RegularExpressions;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.exceptions;
using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Infrastructures;

public class IniConfigurationReader : IConfigurationReader
{
  private readonly IDictionary<string, string> _data = new Dictionary<string, string>();
  public IniConfigurationReader(string fileName)
  {
    var path = Directory.GetCurrentDirectory() + "/" + fileName;
    if (!File.Exists(path) )
    {
      throw new FileNotFoundException("monitor: configuration file not found.");
    }

    ExtractData(path);
  }
  
  private void ExtractData(string path)
  {
    try
    {
      //TODO With or without braces ?
      using ( var reader = new StreamReader(path) )
      {
        var fileToString = reader.ReadToEnd();

        if ( !Regex.IsMatch(fileToString, ReaderFormat.IniFormatRegex) )
          throw new UnableToExtractDataException("monitor: missing configuration file argument");
      
        var lines = fileToString.Split('\n');

        foreach (var line in lines)
        {
          var parts = line.Split('=');
          if (parts.Length == 1)
            continue;
        
          _data.Add(parts[0], parts[1].Trim());
        }
      }
    }
    catch (IOException)
    {
      throw new UnableToExtractDataException("monitor: something went wrong when extracting data.");
    }
  }

  public Configuration GetConfiguration()
  {
    var measurementsExtractor = new MeasurementExtractor(_data["measures"]);
    _data.Remove("measures");
    var alertsExtractor = _data["alerts"];
    _data.Remove("alerts");

    ISet<Thermometer> thermometers = new SortedSet<Thermometer>();
    foreach (var name in _data.Values)
    {
      thermometers.Add(new Thermometer(name));
    }
    
    return new Configuration(thermometers, measurementsExtractor);
  }
}