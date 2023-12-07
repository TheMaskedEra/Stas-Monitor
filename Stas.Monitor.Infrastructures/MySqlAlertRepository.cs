using MySql.Data.MySqlClient;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Domains.Repository.Exceptions;

namespace Stas.Monitor.Infrastructures;

public class MySqlAlertRepository : IAlertRepository
{
    private readonly string _connectionString;
    public MySqlAlertRepository(IDictionary<string, string> connectionData)
    {
        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
    }

    public Alert? RetrieveAlert(int measurementId)
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
                        return reader.Read() ? new Alert(reader.GetDouble("expected_value")) : null;
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
