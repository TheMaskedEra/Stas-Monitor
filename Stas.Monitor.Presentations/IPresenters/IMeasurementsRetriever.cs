namespace Stas.Monitor.Presentations.IPresenters;

public interface IMeasurementsRetriever
{
    public IList<MeasurementViewModel> RetrieveMeasurements(string thermometer, string type, double timer);
}
