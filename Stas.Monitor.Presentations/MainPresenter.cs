using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly IMainView _view;
  private readonly IMeasurementRepository _measurementRepository;
  private readonly IDictionary<string, int> timers;
  public int CurrentTimer { get; set; }

  public MainPresenter(IMainView view, IMeasurementRepository measurementRepository, ISet<string> thermometersNames)
  {
      _view = view;
      _measurementRepository = measurementRepository;

      _view.SelectedThermometer += OnSelectThermometer;

      _view.SelectedTimer += (sender, e) =>
      {
          CurrentTimer = timers[e];
      };

      //TODO c'est pas à config de le faire ?
      timers = new Dictionary<string, int> { { "30 seconds", 30 }, { "1 minute", 60 }, { "5 minutes", 300 } };

      _view.SetupSettings(thermometersNames, timers.Keys);
  }

  public void CheckForUpdates()
  {
      throw new NotImplementedException();
  }

  private void OnSelectThermometer(object? sender, string name)
  {
      var types = _measurementRepository.GetMeasurementTypesFromThermometer(name);

      foreach (var type in types)
      {
          var measurements = _measurementRepository.GetMeasurementsByThermometer(name, type, CurrentTimer);

          var viewModels = measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
          _view.CreateType(type, viewModels);
      }
  }
}

