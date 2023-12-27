using System.Data.Common;
using System.Data.SQLite;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;
using Stas.Monitor.Infrastructures.Database;

namespace Stas.Monitor.Infrastructures.Tests.Database;

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

        InsertMeasurements("Living Room", "Temperature");
    }

    private void InsertMeasurements(string room, string type)
    {
        using var connection = new SQLiteConnection(_connectionString);

        connection.Open();
        for (var i = 0; i <= 10; i += 2)
        {
            var datetime = new DateTime(2023, 12, 16, 19, 50, i);
            var query = $"INSERT INTO Measurements(room, type, value, datetime) values ('{room}', '{type}', 20, '{datetime:yyyy-MM-dd HH:mm:ss}')";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    [Test]
    public void Should_Return_Valid_Measurement_List()
    {
        var result = _databaseMeasurementRepository.GetMeasurementsByThermometer("Living Room","Temperature", 30);
        var expectedResult = new List<Measurement>
        {
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 10), 20, null),
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 8), 20, null),
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 6), 20, null),
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 4), 20, null),
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 2), 20, null),
            new ("Temperature", new DateTime(2023, 12, 16, 19, 50, 0), 20, null),
        };

        CollectionAssert.AreEqual(expectedResult, result);
    }

    [Test]
    public void Should_Throw_Exception_On_Bad_Link()
    {
        const string badLink = "I'm a bad link";

        var factory = DbProviderFactories.GetFactory("System.Data.SQLite");

        Assert.Throws<MeasurementRepositoryException>(() =>
        {
            var databaseMeasurementRepository = new DatabaseMeasurementRepository(factory, new SqlLiteQueries(), badLink);
        });
    }
}
