namespace Stas.Monitor.Presentations;

public interface IMeasurementPresenter
{
    string CurrentThermometer { get; set; }

    TimeSpan CurrentTimeSpan { get; set; }

    IEnumerable<MeasurementViewModel> RetrieveThermometerMeasurements();
}
