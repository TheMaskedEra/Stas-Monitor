using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IPresenters;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly IMainView _view;
  private readonly IMeasurementPresenter _measurementPresenter;

  public MainPresenter(IMainView view, IMeasurementPresenter measurementPresenter, IEnumerable<string> thermometersNames, TimerHandler timerHandler)
  {
      _view = view;
      _measurementPresenter = measurementPresenter;

      _view.SelectedThermometer += OnSelectThermometer;

      _view.SelectedTimer += (sender, e) =>
      {
          var timer = timerHandler.GetTimer(e);
          _measurementPresenter.CurrentTimer = timer.TotalSeconds;
          RefreshLists();
      };

      _view.SetupSettings(thermometersNames, timerHandler.DisplayTimers, measurementPresenter.Types);
      RefreshLists();
  }

  private void OnSelectThermometer(object? sender, string name)
  {
      _measurementPresenter.CurrentThermometer = name;
      RefreshLists();
  }

  public void RefreshLists() => _view.RefreshLists(_measurementPresenter.RetrieveThermometerMeasurements());
}
