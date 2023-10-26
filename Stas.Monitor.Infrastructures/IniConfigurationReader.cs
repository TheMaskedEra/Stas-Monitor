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
  
  //TODO faire ReadToEnd ?
  public Configuration GetData()
  {
    try
    {
      using var reader = new StreamReader(_pathFile);
      var fileToString = reader.ReadToEnd();

      if ( !Regex.IsMatch(fileToString, ReaderFormat.IniFormatRegex) )
        throw new UnableToExtractDataException("monitor: missing configuration file argument");

      IDictionary<string, string> iniValues = new Dictionary<string, string>();
      var lines = fileToString.Split('\n');

      foreach (var line in lines)
      {
        var parts = line.Split('=');
        if (parts.Length == 1)
          continue;
        
        iniValues.Add(parts[0], parts[1]);
      }
      
      //TODO utile ?
      reader.Close();
    }
    catch (IOException e)
    {
      throw new UnableToExtractDataException("monitor: something went wrong when extracting data.");
    }
    return new Configuration();
  }
}