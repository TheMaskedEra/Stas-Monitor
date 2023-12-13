using System.Data;
using System.Data.Common;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures;

public class MeasurementRepository : IMeasurementRepository
{
    private DbProviderFactory Factory { get; }

    private IQueries Queries { get; }

    private string ConnectionString { get; }

    public MeasurementRepository(DbProviderFactory factory, IQueries queries, IDictionary<string, string> connectionData)
    {
        Factory = factory;
        Queries = queries;

        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        ConnectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
    }

    public MeasurementRepository(DbProviderFactory factory, IQueries queries, string connectionString)
    {
        Factory = factory;
        Queries = queries;
        ConnectionString = connectionString;
    }

    public IEnumerable<string> GetMeasurementTypesFromThermometer(string thermometer)
    {
        var types = new List<string>();

        using (var connection = Factory.CreateConnection())
        {
            try
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = Queries.SelectType;
                    CreateParameter(command, "@thermometer", thermometer, DbType.String);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(reader.GetString(0));
                        }

                        return types;
                    }
                }
            }
            catch (Exception e)
            {
                throw new CannotRetrieveMeasurementTypesException("Stas : Something went wrong while trying to retrieve measurement types : " + e.Message);
            }
        }
    }

    public IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, string type, int limit)
    {
        using (var connection = Factory.CreateConnection())
        {
            try
            {
                connection.ConnectionString = ConnectionString;
                var measurements = new List<Measurement>();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = Queries.SelectMeasurements;
                    CreateParameter(command, "@thermometer", thermometer, DbType.String);
                    CreateParameter(command, "@type", type, DbType.String);
                    CreateParameter(command, "@limit", limit, DbType.Int16);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            measurements.Add(InstantiateMeasurement(reader, type));
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

    private Measurement InstantiateMeasurement(DbDataReader reader, string type)
    {
        double? expectedValue = null;
        if (!reader.IsDBNull("expected_value"))
        {
            expectedValue = reader.GetDouble("expected_value");
        }

        var moment = reader.GetDateTime("datetime");
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
