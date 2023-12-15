using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations;

public class MainPresenter
{
  private readonly IMainView _view;
  private readonly MeasurementPresenter _measurementPresenter; //TODO ???

  public MainPresenter(IMainView view, MeasurementPresenter measurementPresenter, IEnumerable<string> thermometersNames, IEnumerable<TimeSpan> timers)
  {
      _view = view;
      _measurementPresenter = measurementPresenter;

      _view.SelectedThermometer += OnSelectThermometer;

      _view.SelectedTimer += (sender, e) =>
      {
          _measurementPresenter.CurrentTimeSpan = TimeSpan.FromSeconds(e);
          RefreshLists();
      };

      _view.SetupSettings(thermometersNames, SetupTimeSpans(timers));
  }

  private void OnSelectThermometer(object? sender, string name)
  {
      _measurementPresenter.CurrentThermometer = name;
      var thermometerMeasurements = _measurementPresenter.RetrieveThermometerMeasurements();

      _view.GenerateThermometerSettings(thermometerMeasurements);
  }

  public void RefreshLists() => _view.UpdateLists(_measurementPresenter.RetrieveThermometerMeasurements());

  private IList<(int Value, string DisplayText)> SetupTimeSpans(IEnumerable<TimeSpan> timeSpan) => timeSpan.Select(timer => ((int)timer.TotalSeconds, TimeSpanFormatter.ToString(timer))).ToList();
}

