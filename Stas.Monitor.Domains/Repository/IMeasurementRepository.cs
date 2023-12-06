namespace Stas.Monitor.Domains.Repository;

public interface IMeasurementRepository
{
    IEnumerable<string> GetMeasurementTypesFromThermometer(string thermometer);

    IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, string type);
}
