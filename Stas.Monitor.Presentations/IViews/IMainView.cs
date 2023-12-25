namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;

  event EventHandler<string> SelectedTimer;

  void SetupSettings(IEnumerable<string> names, IEnumerable<string> displayTimers);

  void GenerateThermometerSettings(IDictionary<string, IList<MeasurementViewModel>> measurements);
}
