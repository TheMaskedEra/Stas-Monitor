using Stas.Monitor.Domains;

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

  //Une seule loop qui initalise d'abord les noms et ensuite les données, utilisation des ifs
  public Configuration ConfigureData()
  {
    IDictionary<string, string> thermometers = new Dictionary<string, string>();
    try
    {
      using ( var reader = new StreamReader(_pathFile) )
      {
      
      }
    }
    catch (IOException e)
    {
      
    }
    return new Configuration(thermometers);
  }
}