namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;

  event EventHandler<int> SelectedTimer;

  IDictionary<string, IList<MeasurementViewModel>> MeasurementLists { get; set; } //TODO reaction event ou trop mvvm ?

  void SetupSettings(IEnumerable<string> names, IList<(int Value, string DisplaySettings)> timers);

  void GenerateThermometerSettings(IDictionary<string, IEnumerable<MeasurementViewModel>> measurements);
}
