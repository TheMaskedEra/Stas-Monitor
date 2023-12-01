using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Infrastructures;

public class MySqlMeasurementRepository : IMeasurementRepository
{
    private readonly string _connectionString;

    public MySqlMeasurementRepository(IDictionary<string, string> connectionData)
    {
        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Port={port};Database={database};User ID={user};Password={password};";
    }

    public IEnumerable<Measurement> GetMeasurementsByThermometer(Thermometer thermometer)
    {
        throw new NotImplementedException();
    }

    private Alert RetrieveAlert(int measurementID)
    {
        throw new NotImplementedException();
    }
}
