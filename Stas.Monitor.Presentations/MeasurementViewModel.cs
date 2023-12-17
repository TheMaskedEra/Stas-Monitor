using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MeasurementViewModel
{
  private readonly Measurement _measurement;

  public MeasurementViewModel(Measurement measurement)
  {
    _measurement = measurement;
  }

  public string Value => GetFormattedValue(_measurement.Value);

  public string Moment => _measurement.Moment.ToString("dddd d MMMM yyyy hh:mm:ss");

  public string ExpectedValue
  {
    get
    {
        if (_measurement.AlertOffset == null)
        {
            return string.Empty;
        }

        var value = (double) _measurement.AlertOffset;
        return "Expected : " + GetFormattedValue(value);
    }
  }

  public string? Background => _measurement.AlertOffset != null ? "red" : null;

  private string GetFormattedValue(double value)
  {
      var format = _measurement.Type switch
      {
          "Temperature" => "00.0°C",
          "Humidity" => "00%",
          _ => string.Empty,
      };

      return value.ToString(format);
  }

  public string Type => _measurement.Type;
}
