using System.Data.Common;
using System.Data.SQLite;

namespace Stas.Monitor.Infrastructures.Tests;

public class DatabaseMeasurementRepositoryTests
{
    private DatabaseMeasurementRepository _databaseMeasurementRepository;
    private string _connectionString;
    private IQueries _queries;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _connectionString = "Data Source=database.sqlite;Version=3;";
        DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
        _queries = new SqlLiteQueries();

    }

    [SetUp]
    public void Setup()
    {
        var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
        _databaseMeasurementRepository = new DatabaseMeasurementRepository(factory, _queries, _connectionString);
    }
}
