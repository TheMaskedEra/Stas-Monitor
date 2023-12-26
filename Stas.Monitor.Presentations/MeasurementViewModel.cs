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

  public string? Background
  {
      get
      {
          if (_measurement.AlertOffset == null)
          {
              return null;
          }

          return _measurement.AlertOffset < _measurement.Value ? "red" : "blue";
      }
  }

  private string GetFormattedValue(double value)
  {
      var format = _measurement.Type switch
      {
          "Temperature" => "0.0°C",
          "Humidity" => "0.0%",
          _ => "0.0",
      };

      return value.ToString(format, CultureInfo.InvariantCulture);
  }

  public string Type => _measurement.Type;

  public override bool Equals(object? obj)
  {
      if (obj == null || GetType() != obj.GetType())
      {
          return false;
      }

      var other = (MeasurementViewModel)obj;
      return Value == other.Value
          && Moment == other.Moment
                      && ExpectedValue == other.ExpectedValue
                      && Background == other.Background
                      && Type == other.Type;
  }

  public override int GetHashCode() => HashCode.Combine(Value, Moment, ExpectedValue, Background, Type);
}
