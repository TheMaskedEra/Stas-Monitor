namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;

  event EventHandler<bool> TemperatureCheck;

  event EventHandler<bool> HumidityCheck;

  void SetNames(ISet<string> names); //TODO pourquoi enumerable ?

  void ShowMeasurements(IEnumerable<MeasurementViewModel> measures);
}
