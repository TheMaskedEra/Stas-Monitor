using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly Configuration _config;
  private readonly IMainView _view;
  private EventHandler<string> selectedThermometer;
  
  private Thermometer _currentThermometer;
  public MainPresenter(Configuration configuration, IMainView view)
  {
    _config = configuration;
    
    _view = view;
    selectedThermometer = OnSelectThermometer;
    _view.SelectedThermometer += selectedThermometer;
    
    _view.SetNames(_config.Names);
  }

  private void OnSelectThermometer(object sender, string name)
  {
    _currentThermometer = _config.FindThermometer(name)!;
    var measurements = _config.GetMeasurement(_currentThermometer);
    var measureViewModels = TranslateToDto(measurements);
    _view.ShowMeasurements(measureViewModels);
  }

  private List<MeasurementViewModel> TranslateToDto(IEnumerable<IStatement> measurements)
  {
    return measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
  }
}