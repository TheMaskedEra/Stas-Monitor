namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;

  event EventHandler<int> SelectedTimer;

  void SetupSettings(IEnumerable<string> names, IList<(int Value, string DisplaySettings)> timers);

  void GenerateThermometerSettings(IDictionary<string, IEnumerable<MeasurementViewModel>> measurements);

  void UpdateLists(IDictionary<string, IEnumerable<MeasurementViewModel>> viewModels);
}
