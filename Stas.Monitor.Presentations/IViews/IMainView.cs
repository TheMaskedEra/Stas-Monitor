namespace Stas.Monitor.Presentations.IViews;

public interface IMainView
{
  event EventHandler<string> SelectedThermometer;

  void SetNames(ISet<string> names); //TODO pourquoi enumerable ?

  void CreateType(string typeName, IEnumerable<MeasurementViewModel> measurements);
}
