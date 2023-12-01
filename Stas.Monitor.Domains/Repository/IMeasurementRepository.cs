namespace Stas.Monitor.Domains.Repository;

public interface IMeasurementRepository
{
    IEnumerable<Measurement> GetMeasurementsByThermometer(Thermometer thermometer);
}
