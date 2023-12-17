namespace Stas.Monitor.Domains.Tests;

public class ConfigurationTests
{
    [Test]
    public void Configuration_WhenCreated_ShouldHaveCorrectProperties()
    {
        // Arrange
        ISet<string> thermometerNames = new HashSet<string> { "Living Room", "Bedroom" };
        IDictionary<string, string> data = new Dictionary<string, string> { { "csvPath", "link" }, { "dbPath", "link" } };
        var timers = new HashSet<int> { 30, 60, 300 };
        var timerHandler = new TimerHandler(timers);

        // Act
        var configuration = new Configuration(thermometerNames, data, timerHandler);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(configuration.ThermometerNames, Is.EqualTo(thermometerNames));
            Assert.That(configuration.Data, Is.EqualTo(data));
            Assert.That(configuration.TimerHandler, Is.EqualTo(timerHandler));
        });
    }
}
