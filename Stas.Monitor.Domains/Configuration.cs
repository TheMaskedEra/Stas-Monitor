using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Domains;

public record Configuration
{
  private readonly ISet<Thermometer> _thermometers;
  private readonly IDictionary<string, string> _connectionData;

  public Configuration(ISet<Thermometer> thermometers, IDictionary<string, string> connectionData)
  {
    _thermometers = thermometers;
    _connectionData = connectionData;
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

  public IDictionary<string, string> ConnectionData => _connectionData;

  public Thermometer? FindThermometer(string name) => _thermometers.FirstOrDefault(thermometer => thermometer.Name.Equals(name));
}
