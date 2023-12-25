namespace Stas.Monitor.Presentations.IPresenters;

public interface IMeasurementPresenter
{
    string CurrentThermometer { set; }

    double CurrentTimer { set; }

    string[] Types { get; init; }

    IDictionary<string, IList<MeasurementViewModel>> RetrieveThermometerMeasurements();
}
