namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;
  void SetNames(ISet<string> names);
  void ShowMeasurements(IEnumerable<MeasurementViewModel> measures);
}