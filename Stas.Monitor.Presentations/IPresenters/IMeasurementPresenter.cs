namespace Stas.Monitor.Presentations.IPresenters;

public interface IMeasurementPresenter
{
    string CurrentThermometer { set; }

    double CurrentTimer { set; }

    IDictionary<string, IList<MeasurementViewModel>> RetrieveThermometerMeasurements();
}
