using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Domains;

public class Configuration
{
  private readonly IStatementReader _measurementReader;
  private readonly ISet<Thermometer> _thermometers;
  public Configuration(ISet<Thermometer> thermometers, IStatementReader measurementReader)
  {
    _thermometers = thermometers;
    _measurementReader = measurementReader;
  }

  public ISet<string> Names
  {
    get
    {
      ISet<string> names = new SortedSet<string>();
      foreach ( var thermometer in _thermometers )
      {
        names.Add(thermometer.Name);
      }

      return names;
    }
  }
  public Thermometer? FindThermometer(string name)
  {
    return _thermometers.FirstOrDefault(thermometer => thermometer.Name.Equals(name));
  }

  public IEnumerable<IStatement> GetMeasurement(Thermometer thermometer)
  {
    return _measurementReader.GetStatements(thermometer);
  }
}