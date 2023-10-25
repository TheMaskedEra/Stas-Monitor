using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly Configuration _config;
  private IMainView _view;
  
  private Thermometer _currentThermometer;
  public MainPresenter(Configuration configuration)
  {
    _config = configuration;
  }

  public void SetView(IMainView view)
  {
    _view = view;
    _view.SetNames(_config.Names);
  }

  public void SelectThermometer(string name)
  {
    var info = name + " sélectionné";
    _view.ShowCurrentThermometer(info);
  }
}