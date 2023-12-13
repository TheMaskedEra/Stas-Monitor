using System.Globalization;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MeasurementViewModel
{
  private readonly Measurement _measurement;

  public MeasurementViewModel(Measurement measurement)
  {
    _measurement = measurement;
  }

  public string Value => GetFormattedValue();

  public string Moment => _measurement.Moment.ToString("dddd d MMMM yyyy hh:mm:ss");

  public string ExpectedValue
  {
    get
    {
      if ( _measurement.AlertOffset != null )
      {
        return "Expected : " + _measurement.AlertOffset;
      }

      return string.Empty;
    }
  }

  public string? Background => _measurement.AlertOffset != null ? "red" : null;

  private string GetFormattedValue()
  {
      var format = _measurement.Type switch
      {
          "Temperature" => "00.0°C",
          "Humidity" => "00%",
          _ => string.Empty,
      };

      return _measurement.Value.ToString(format);
  }
}
