using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly Configuration _config;
  private readonly IMainView _view;
  private EventHandler<string> selectedThermometer;
  
  private Thermometer? _currentThermometer;
  public MainPresenter(Configuration configuration, IMainView view)
  {
    _config = configuration;
    
    _view = view;
    selectedThermometer = OnSelectThermometer!;
    _view.SelectedThermometer += selectedThermometer;
    
    _view.SetNames(_config.Names);
  }

  private void OnSelectThermometer(object sender, string name)
  {
    _currentThermometer = _config.FindThermometer(name)!;
    var measurements = _config.GetMeasurement(_currentThermometer);
    var alerts = _config.GetAlerts(_currentThermometer);

    var viewModels = MergeStatements(measurements.ToList(), alerts.ToList());
    _view.ShowMeasurements(viewModels);
  }

  private IEnumerable<StatementViewModel> MergeStatements(List<IStatement> measurements, List<IStatement> alerts)
  {
    SortStatements(measurements, alerts);
    var statementViewModels = ( from Measurement? measurement in measurements select new StatementViewModel(measurement) ).ToList();

    statementViewModels.AddRange(from Alert? alert in alerts select new StatementViewModel(alert));

    return statementViewModels;
  }

  private void SortStatements(ICollection<IStatement> measurements, List<IStatement> alerts)
  {
    var measurementsCopy = new List<IStatement>(measurements);
    foreach (var expectedMeasurement in from expectedMeasurement in measurementsCopy let measurement = (Measurement)expectedMeasurement 
             let moment = measurement.Moment from alert in alerts.Cast<Alert>().Where(alert => moment == alert.Moment) select expectedMeasurement)
    {
      measurements.Remove(expectedMeasurement);
    }
  }
}