using System.Data.Common;
using System.Data.SQLite;
using Stas.Monitor.Domains.Repository.Exceptions;
using Stas.Monitor.Infrastructures.Database;

namespace Stas.Monitor.Infrastructures.Tests;

public class DatabaseMeasurementRepositoryTests
{
    private readonly string _connectionString = $"Data Source={Directory.GetCurrentDirectory()}/resources/Database/database.sqlite";
    private DatabaseMeasurementRepository _databaseMeasurementRepository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
        var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
        var queries = new SqlLiteQueries();

        _databaseMeasurementRepository = new DatabaseMeasurementRepository(factory, queries, _connectionString);
    }

    private static string ResetDataBase => "DROP TABLE IF EXISTS Measurements;" +
                                           "DROP TABLE IF EXISTS Alerts;" +
                                           "CREATE TABLE Measurements (" +
                                           "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                           "room TEXT NOT NULL," +
                                           "type TEXT NOT NULL," +
                                           "value REAL NOT NULL," +
                                           "datetime TEXT NOT NULL" +
                                           ");" +
                                           "CREATE TABLE Alerts(" +
                                           "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                           "expected_value INTEGER NOT NULL," +
                                           "measurement_id INTEGER NOT NULL," +
                                           "FOREIGN KEY(measurement_id)" +
                                           "REFERENCES Measurements (id)" +
                                           ");";

    [SetUp]
    public void Setup()
    {
        using var connection = new SQLiteConnection(_connectionString);

        connection.Open();
        using var command = new SQLiteCommand(ResetDataBase, connection);
        command.ExecuteNonQuery();
        connection.Close();

        Insert30SecondsMeasurements("Living Room");
        Insert1MinuteMeasurements("Living Room");
        Insert5MinuteMeasurements("Living Room");

        Insert30SecondsMeasurements("Bedroom");
    }

    private void Insert30SecondsMeasurements(string room)
    {
        using var connection = new SQLiteConnection(_connectionString);

        connection.Open();
        for (var i = 5; i < 30; i++)
        {
            var datetime = new DateTime(2023, 12, 16, 19, 50, i);
            var query = $"INSERT INTO Measurements(room, type, value, datetime) values ('{room}', 'Temperature', 20, '{datetime:yyyy-MM-dd HH:mm:ss}')";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    private void Insert1MinuteMeasurements(string room)
    {
        using var connection = new SQLiteConnection(_connectionString);

        connection.Open();
        for (var i = 35; i < 45; i++)
        {
            var datetime = new DateTime(2023, 12, 16, 19, 49, i);
            var query = $"INSERT INTO Measurements(room, type, value, datetime) values ('{room}', 'Temperature', 20, '{datetime:yyyy-MM-dd HH:mm:ss}')";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    private void Insert5MinuteMeasurements(string room)
    {
        using var connection = new SQLiteConnection(_connectionString);

        connection.Open();
        for (var i = 46; i < 49; i++)
        {
            var datetime = new DateTime(2023, 12, 16, 19, i, 05);
            var query = $"INSERT INTO Measurements(room, type, value, datetime) values ('{room}', 'Temperature', 20, '{datetime:yyyy-MM-dd HH:mm:ss}')";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    [Test]
    public void Should_Return_Valid_Measurement_List()
    {
        var measurementSize30Secs = _databaseMeasurementRepository.GetMeasurementsByThermometer("Living Room", 30);
        var measurementSize1Min = _databaseMeasurementRepository.GetMeasurementsByThermometer("Living Room", 60);
        var measurementSize5Min = _databaseMeasurementRepository.GetMeasurementsByThermometer("Living Room", 300);

        Assert.Multiple(() =>
        {
            Assert.That(measurementSize30Secs.Count(), Is.EqualTo(25));
            Assert.That(measurementSize1Min.Count(), Is.EqualTo(35));
            Assert.That(measurementSize5Min.Count(), Is.EqualTo(38));
        });
    }

    [Test]
    public void Should_Throw_Exception_On_Bad_Link()
    {
        const string badLink = "I'm a bad link";

        try
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            var repo = new DatabaseMeasurementRepository(factory, new SqlLiteQueries(), badLink);
        }
        catch (MeasurementRepositoryException)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
}
