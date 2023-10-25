using System.Text.RegularExpressions;
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
  
  public Configuration GetData()
  {
    ISet<Thermometer> thermometers = new HashSet<Thermometer>();
    string measures, alerts;
    try
    {
      using var reader = new StreamReader(_pathFile);
      
      ParseThermometersNames(reader, thermometers);
      
      while ( reader.ReadLine() is { } line )
      {
        if ( Regex.IsMatch(line, ReaderFormat.MeasureFormat) )
          measures = line[(line.IndexOf('=')+1)..];
        if ( Regex.IsMatch(line, ReaderFormat.AlertFormat) )
          alerts = line[(line.IndexOf('=')+1)..];
      }
      
    }
    catch (IOException e)
    {
      throw new UnableToExtractDataException("monitor: something went wrong when extracting data.");
    }
    return new Configuration(thermometers);
  }

  private void ParseThermometersNames(TextReader reader, ISet<Thermometer> thermometers)
  {
    var line = reader.ReadLine();
    if (line is null || !line.Equals("[thermometers]"))
      throw new UnableToExtractDataException("monitor: missing required section thermometers");
    
    while ( (line = reader.ReadLine()) is not null)
    {
      if ( line.Equals("[paths]") ) break;

      if ( !Regex.IsMatch(line, ReaderFormat.NameFormat) ) continue;
      
      var name = line[(line.IndexOf('=')+1)..];
      thermometers.Add(new Thermometer(name));
    }
  }
}