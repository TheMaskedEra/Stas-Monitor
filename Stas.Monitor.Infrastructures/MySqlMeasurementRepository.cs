using MySql.Data.MySqlClient;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures;

public class MySqlMeasurementRepository : IMeasurementRepository
{
    private readonly string _connectionString;
    private readonly IAlertRepository _alertRepository;

    public MySqlMeasurementRepository(IDictionary<string, string> connectionData, IAlertRepository alertRepository)
    {
        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
        _alertRepository = alertRepository;
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
                            //TODO
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

    public IEnumerable<Measurement> GetMeasurementsByThermometer(string thermometer, string type)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                const string query = "SELECT * FROM Measurements " +
                                     "WHERE room = @thermometer AND type = @type " +
                                     "ORDER BY datetime DESC";
                var measurements = new List<Measurement>();
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("thermometer", thermometer);
                    command.Parameters.AddWithValue("type", type);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //TODO
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

    private Alert? RetrieveAlert(int measurementId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                const string query = "SELECT * FROM Alerts WHERE measurement_id = @measurementID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("measurementID", measurementId);
                    using (var reader = command.ExecuteReader())
                    {
                        //TODO
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw new CannotRetrieveAlertException("Stas : Something went wrong while trying to retrieve an alert : " + e.Message);
            }
        }
    }
}
