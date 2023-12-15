using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly IMainView _view;
  private readonly MeasurementPresenter _measurementPresenter; //TODO ???

  public MainPresenter(IMainView view, MeasurementPresenter measurementPresenter, IEnumerable<string> thermometersNames, TimerHandler timerHandler)
  {
      _view = view;
      _measurementPresenter = measurementPresenter;

      _view.SelectedThermometer += OnSelectThermometer;

      _view.SelectedTimer += (sender, e) =>
      {
          _measurementPresenter.CurrentTimeSpan = timerHandler.GetTimer(e);
          RefreshLists();
      };

      _view.SetupSettings(thermometersNames, timerHandler.DisplayTimers);
  }

  private void OnSelectThermometer(object? sender, string name)
  {
      _measurementPresenter.CurrentThermometer = name;
      var thermometerMeasurements = _measurementPresenter.RetrieveThermometerMeasurements();

      _view.GenerateThermometerSettings(thermometerMeasurements);
  }

  public void RefreshLists() => _view.UpdateLists(_measurementPresenter.RetrieveThermometerMeasurements());
}
