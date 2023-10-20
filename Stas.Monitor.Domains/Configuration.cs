namespace Stas.Monitor.Domains;

public class Configuration
{
  public Configuration(IDictionary<string, string> thermometers)
  {
    Thermometers = thermometers;
  }

  public Thermometer CurrentThermometer
  {
    get;
    set;
  }

  public IDictionary<string, string> Thermometers
  {
    get;
  }
}