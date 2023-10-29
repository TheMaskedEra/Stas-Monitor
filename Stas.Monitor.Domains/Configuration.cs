using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Domains;

public class Configuration
{
  private readonly IMeasurementExtractor _measurementExtractor;
  public Configuration(ISet<Thermometer> thermometers, IMeasurementExtractor measurementExtractor)
  {
    Thermometers = thermometers;
    _measurementExtractor = measurementExtractor;
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
  public Thermometer? FindThermometer(string name)
  {
    return Thermometers.FirstOrDefault(thermometer => thermometer.Name.Equals(name));
  }

  public IEnumerable<Measurement> GetMeasurement(Thermometer thermometer)
  {
    return _measurementExtractor.GetMeasurements(thermometer);
  }
}