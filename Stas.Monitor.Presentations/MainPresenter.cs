using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly IMainView _view;
  private readonly IMeasurementRepository _measurementRepository;

  public MainPresenter(IMainView view, IMeasurementRepository measurementRepository, ISet<string> thermometersNames)
  {
      _view = view;
      _measurementRepository = measurementRepository;

      _view.SelectedThermometer += OnSelectThermometer;

      _view.SetNames(thermometersNames);
  }

  private void OnSelectThermometer(object? sender, string name)
  {
      var types = _measurementRepository.GetMeasurementTypesFromThermometer(name);

      foreach (var type in types)
      {
          var measurements = _measurementRepository.GetMeasurementsByThermometer(name, type);

          var viewModels = measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
          _view.CreateType(type, viewModels);
      }
  }
}
