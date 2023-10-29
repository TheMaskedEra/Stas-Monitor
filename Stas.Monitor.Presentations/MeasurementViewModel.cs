using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MeasurementViewModel
{
  private readonly Measurement _measurement;
  public MeasurementViewModel(IStatement measurement)
  {
    if ( measurement is not Measurement statement )
      throw new ArgumentException("Parameter should be Measurement");
    
    _measurement = statement;
  }
  
  public string Name => _measurement.Name;
  public string Type => _measurement.Type;
  public string Value => _measurement.Value + "°";
  public string Moment => _measurement.Moment.ToString("dddd d MMMM yyyy hh:mm:ss");
}