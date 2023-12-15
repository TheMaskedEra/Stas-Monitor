namespace Stas.Monitor.Domains.Repository;

public interface IMeasurementRepository
{
    IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, double totalSeconds);
}
