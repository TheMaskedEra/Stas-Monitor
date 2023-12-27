using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;
using Stas.Monitor.Infrastructures.Ini;

namespace Stas.Monitor.Infrastructures.Tests.Ini;

public class Tests
{
    private readonly string _filePath = $"{Directory.GetCurrentDirectory()}/resources/configs/config.ini";

    [Test]
    public void Returns_Valid_Configuration()
    {
        var reader = new IniConfigurationReader(_filePath);
        var config = reader.RetrieveConfiguration();

        var thermometers = config.ThermometerNames;
        var expectedThermometers = new HashSet<string> { "Living Room", "Kitchen", "Bedroom" };
        CollectionAssert.AreEquivalent(thermometers, expectedThermometers);

        var measurementsData = config.Data;
        var expectedMeasurementData = new Dictionary<string, string>
        {
            { "host", "192.168.132.200" },
            { "port", "13306" },
            { "database", "E200971" },
            { "user", "E200971" },
            { "password", "0971" }
        };
        CollectionAssert.AreEquivalent(measurementsData, expectedMeasurementData);

        var timerHandler = config.TimerHandler;
        var expectedTimerHandler = new TimerHandler(new List<int> { 30, 60, 300 });
        Assert.That(timerHandler, Is.EqualTo(expectedTimerHandler));
    }

    [Test]
    public void Throws_Error_On_File_Not_Found()
    {
        const string badFilePath = "I'm a bad file path.";

        var badReader = new IniConfigurationReader(badFilePath);

        Assert.Throws<ConfigurationReaderException>(() => badReader.RetrieveConfiguration());
    }
}
