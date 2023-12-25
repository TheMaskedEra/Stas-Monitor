using System.Data;
using System.Data.Common;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures.Database;

public class DatabaseMeasurementRepository : IMeasurementRepository
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly IQueries _queries;

    public DatabaseMeasurementRepository(DbProviderFactory factory, IQueries queries, IDictionary<string, string> connectionData)
    {
        _factory = factory;
        _queries = queries;
        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";

        TestConnection();
    }

    public DatabaseMeasurementRepository(DbProviderFactory factory, IQueries queries, string connectionString)
    {
        _factory = factory;
        _queries = queries;
        _connectionString = connectionString;

        TestConnection();
    }

    private void TestConnection()
    {
        using var connection = _factory.CreateConnection();
        try
        {
            connection!.ConnectionString = _connectionString;
            connection.Open();
        }
        catch (Exception)
        {
            throw new MeasurementRepositoryException("Stas : unable to connect to the database");
        }
    }

    public IList<Measurement> GetMeasurementsByThermometer(string thermometer, string type, double totalSeconds)
    {
        using var connection = _factory.CreateConnection();
        try
        {
            connection!.ConnectionString = _connectionString;
            var measurements = new List<Measurement>();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = _queries.MeasurementSelect;
            CreateParameter(command, "@thermometer", thermometer, DbType.String);
            CreateParameter(command, "@limit", totalSeconds, DbType.Double);
            CreateParameter(command, "@type", type, DbType.String);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                measurements.Add(InstantiateMeasurement(reader));
            }

            return measurements;
        }
        catch (Exception e)
        {
            throw new MeasurementRepositoryException("Stas: unable to read Data : " + e.Message);
        }
    }

    private static Measurement InstantiateMeasurement(DbDataReader reader)
    {
        double? expectedValue = null;
        if (!reader.IsDBNull("expected_value"))
        {
            expectedValue = reader.GetDouble("expected_value");
        }

        var moment = reader.GetDateTime("datetime");
        var type = reader.GetString("type");
        var value = reader.GetDouble("value");
        return new Measurement(type, moment, value, expectedValue);
    }

    private static void CreateParameter(DbCommand command, string name, object value, DbType type)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        parameter.DbType = type;
        command.Parameters.Add(parameter);
    }
}
