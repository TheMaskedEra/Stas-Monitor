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

    public IList<Measurement> GetMeasurementsByThermometer(string thermometer, string type, double totalSeconds) => throw new NotImplementedException();
}
