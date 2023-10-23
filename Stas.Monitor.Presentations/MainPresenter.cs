using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly Configuration _config;
  private Thermometer _currentThermometer;
  public MainPresenter(Configuration configuration )
  {
    _config = configuration;
  }

  public IMainView View
  {
    get;
    set;
  }

  public ISet<string> Names
  {
    get { return _config.Names; }
  }
}