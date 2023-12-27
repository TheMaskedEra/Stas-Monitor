using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;
using Stas.Monitor.Infrastructures.CSV;

namespace Stas.Monitor.Infrastructures.Tests.CSV;

public class CsvMeasurementRepositoryTests
{
    private readonly string _measurementsPath = $"{Directory.GetCurrentDirectory()}/resources/CSV/measurements.csv";
    private readonly string _alertsPath = $"{Directory.GetCurrentDirectory()}/resources/CSV/alerts.csv";

    [Test]
    public void GetMeasurements_Should_ReturnValidListOfMeasurements_WithDifferentTimers()
    {
        var repository = new CsvMeasurementRepository(_measurementsPath, _alertsPath);

        var temperatureFromLivingRoomPast30Sec = repository.GetMeasurementsByThermometer("Living Room", "Temperature", 30);
        var expectedTemperaturesFromLivingRoomPast30Sec = new List<Measurement>
        {
            new("Temperature", new DateTime(2023, 12, 15, 14, 30, 30), 18, null),
            new("Temperature", new DateTime(2023, 12, 15, 14, 30, 28), 15.5, 18)
        };
        CollectionAssert.AreEqual(expectedTemperaturesFromLivingRoomPast30Sec, temperatureFromLivingRoomPast30Sec);
    }

    [Test]
    public void GetMeasurements_Should_ReturnValidListOfMeasurements_WithDifferentTypes()
    {
        var repository = new CsvMeasurementRepository(_measurementsPath, _alertsPath);

        var humidityFromLivingRoomPast30Sec = repository.GetMeasurementsByThermometer("Living Room", "Humidity", 30);
        var expectedHumidityFromLivingRoomPast30Sec = new List<Measurement>
        {
            new("Humidity", new DateTime(2023, 12, 15, 14, 30, 30), 0.35, null),
            new("Humidity", new DateTime(2023, 12, 15, 14, 30, 28), 0.33, null)
        };
        CollectionAssert.AreEqual(expectedHumidityFromLivingRoomPast30Sec, humidityFromLivingRoomPast30Sec);
    }

    [Test]
    public void GetMeasurements_Should_ThrowMeasurementRepositoryException_InvalidMeasurementPath()
    {
        var repository = new CsvMeasurementRepository("Invalid Path", _alertsPath);

        Assert.Throws<MeasurementRepositoryException>(() => repository.GetMeasurementsByThermometer("Living Room", "Temperature", 30));
    }
}
