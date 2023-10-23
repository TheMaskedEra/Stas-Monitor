namespace Stas.Monitor.Domains;

public class Configuration
{
  public Configuration(ISet<Thermometer> thermometers)
  {
    Thermometers = thermometers;
  }
  public ISet<Thermometer> Thermometers
  {
    get;
  }

  public ISet<string> Names
  {
    get
    {
      ISet<string> names = new HashSet<string>();
      foreach ( var thermometer in Thermometers )
      {
        names.Add(thermometer.Name);
      }

      return names;
    }
  }
}