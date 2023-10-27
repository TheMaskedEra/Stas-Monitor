using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Domains;

public class Configuration
{
  public Configuration(ISet<Thermometer> thermometers, IMeasurementExtractor measurementExtractor)
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
      ISet<string> names = new SortedSet<string>();
      foreach ( var thermometer in Thermometers )
      {
        names.Add(thermometer.Name);
      }

      return names;
    }
  }
}