using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Infrastructures.CSV;

public class CsvMeasurementRepository : IMeasurementRepository
{
    private CsvDataReader _csvReader;

    public CsvMeasurementRepository(string filePath)
    {
        _csvReader = new CsvDataReader(filePath);
    }

    public IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, double totalSeconds)
    {
        throw new NotImplementedException();
    }
}
