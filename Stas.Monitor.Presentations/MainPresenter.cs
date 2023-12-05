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

      EventHandler<bool> temperatureCheck = OnTemperatureChecked!;
      _view.TemperatureCheck += temperatureCheck;

      EventHandler<bool> humidityCheck = OnHumidityChecked!;
      _view.HumidityCheck += humidityCheck;

      _view.SetNames(thermometersNames);
  }

  private void OnSelectThermometer(object sender, string name)
  {
    var measurements = _measurementRepository.GetMeasurementsByThermometer(name, "temperature");
    var viewModels = measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();

    _view.ShowMeasurements(viewModels);
  }

  private void OnTemperatureChecked(object sender, bool value)
  {
      Console.WriteLine(value ? "Temperature checked" : "Temperature Unchecked");
  }

  private void OnHumidityChecked(object sender, bool value)
  {
      Console.WriteLine(value ? "Humidity checked" : "Humidity Unchecked");
  }
}
