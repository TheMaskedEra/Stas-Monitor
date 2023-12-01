using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MeasurementViewModel
{
  private readonly Measurement _measurement;

  public MeasurementViewModel(Measurement measurement)
  {
    _measurement = measurement;
  }

  public string Value => _measurement.Value;

  public string Moment => _measurement.Moment.ToString("dddd d MMMM yyyy hh:mm:ss");

  public string ExpectedValue
  {
    get
    {
      if ( _measurement.Alert != null )
      {
        return "Expected : " + _measurement.Alert.ExpectedValue;
      }

      return string.Empty;
    }
  }

  public string? Background => _measurement.Alert != null ? "blue" : null;
}
