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

      EventHandler<string> selectedThermometer = OnSelectThermometer!;
      _view.SelectedThermometer += selectedThermometer;

      _view.SetNames(thermometersNames);
  }

  private void OnSelectThermometer(object sender, string name)
  {
      var types = new List<string> { "Temperature", "Humidity" };//_measurementRepository.GetMeasurementTypesFromThermometer(name);

      //TODO
      foreach (var type in types)
      {
          var measurements = new List<Measurement>
          {
              new (new DateTime(2023, 12, 6, 23, 55, 00), 18.2, null),
              new(new DateTime(2023, 12, 6, 23, 55, 02), 18.2, null),
              new (new DateTime(2023, 12, 6, 23, 55, 04),18.2, null),
              new (new DateTime(2023, 12, 6, 23, 55, 06), 18.2, null),
              new (new DateTime(2023, 12, 6, 23, 55, 08), 18.2, null),
          }; //_measurementRepository.GetMeasurementsByThermometer(name, type);

          //TODO retrieve les alerts non pas pour les measurements mais pour les viewModels ?
          var viewModels = measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
          _view.CreateType(type, viewModels);
      }
  }
}
