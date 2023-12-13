using System.Data;
using System.Data.Common;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures;

public class MeasurementRepository : IMeasurementRepository
{
    private readonly DbProviderFactory _factory;
    private readonly IQueries _queries;
    private readonly string _connectionString;

    public MeasurementRepository(DbProviderFactory factory, IQueries queries, IDictionary<string, string> connectionData)
    {
        _factory = factory;
        _queries = queries;

        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
    }

    public MeasurementRepository(DbProviderFactory factory, IQueries queries, string connectionString)
    {
        _factory = factory;
        _queries = queries;
        _connectionString = connectionString;
    }

    public IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, TimeSpan limit)
    {
        using (var connection = _factory.CreateConnection())
        {
            try
            {
                connection.ConnectionString = _connectionString;
                var measurements = new List<Measurement>();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = _queries.SelectMeasurements;
                    CreateParameter(command, "@thermometer", thermometer, DbType.String);
                    CreateParameter(command, "@limit", limit, DbType.Time);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            measurements.Add(InstantiateMeasurement(reader));
                        }

                        return measurements;
                    }
                }
            }
            catch (Exception e)
            {
                throw new CannotRetrieveMeasurementException("Stas: Something went wrong while trying to retrieve a measurement : " + e.Message);
            }
        }
    }

    private Measurement InstantiateMeasurement(DbDataReader reader)
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

    private void CreateParameter(DbCommand command, string name, object value, DbType type)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        parameter.DbType = type;
        command.Parameters.Add(parameter);
    }
}
