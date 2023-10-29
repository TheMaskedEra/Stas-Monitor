using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IViews;

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
    _currentThermometer = _config.FindThermometer(name)!;
    var measurements = _config.GetMeasurement(_currentThermometer);
    var measureViewModels = TranslateToDto(measurements);
    _view.ShowMeasurements(measureViewModels);
  }

  private List<MeasurementViewModel> TranslateToDto(IEnumerable<Measurement> measurements)
  {
    return measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
  }
}