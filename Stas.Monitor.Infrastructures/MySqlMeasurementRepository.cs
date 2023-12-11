using System.Data;
using MySql.Data.MySqlClient;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

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
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
    }

    public IEnumerable<string> GetMeasurementTypesFromThermometer(string thermometer)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                const string query = "SELECT DISTINCT type " +
                                     "FROM Measurements " +
                                     "WHERE room = @thermometer";
                var types = new List<string>();
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("thermometer", thermometer);
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
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                var query = Query;
                var measurements = new List<Measurement>();
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("thermometer", thermometer);
                    command.Parameters.AddWithValue("type", type);
                    command.Parameters.AddWithValue("limit", limit);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double? expectedValue = null;
                            if (!reader.IsDBNull("expected_value"))
                            {
                                expectedValue = reader.GetDouble("expected_value");
                            }

                            var moment = reader.GetDateTime("datetime");
                            var value = reader.GetDouble("value");

                            measurements.Add(new Measurement(type, moment, value, expectedValue));
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

    private static string Query =>
        "SELECT m.value, m.datetime, a.expected_value " +
        "FROM Measurements m " +
        "LEFT jOIN Alerts a ON m.id = a.measurement_id " +
        "WHERE room = @thermometer AND type = @type " + //AND m.datetime >= NOW() - INTERVAL @limit SECOND " +
        "ORDER BY datetime DESC";
}
