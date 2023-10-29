namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  void SetNames(ISet<string> names);
  void ShowMeasurements(IEnumerable<MeasurementViewModel> measures);
}