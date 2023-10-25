namespace Stas.Monitor.Presentations;

public interface IMainView
{
  void SetNames(IEnumerable<string> names);
  void ShowCurrentThermometer(string message);
}