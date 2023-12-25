namespace Stas.Monitor.Domains.Repository;

public interface IMeasurementRepository
{
    IList<Measurement> GetMeasurementsByThermometer(string thermometer, string type, double totalSeconds);
}
