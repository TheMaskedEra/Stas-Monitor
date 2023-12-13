using System.Data.Common;
using System.Data.SQLite;

namespace Stas.Monitor.Infrastructures.Tests;

public class MeasurementRepositoryTests
{
    private MeasurementRepository measurementRepository;
    private string ConnectionString;
    [SetUp]
    public void Setup()
    {
        ConnectionString = $"Data Source=database.sqlite;Version=3;";
        DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);

        var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
        measurementRepository = new MeasurementRepository(factory, new SQLiteQueries(), ConnectionString);
    }
}
