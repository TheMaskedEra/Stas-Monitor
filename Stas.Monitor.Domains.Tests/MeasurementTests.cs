namespace Stas.Monitor.Domains.Tests;

public class MeasurementTests
{
    [Test]
    public void Measurement_WhenCreated_ShouldHaveCorrectProperties()
    {
        // Arrange
        const string type = "Temperature";
        var moment = DateTime.Now;
        const double value = 25.5;
        double? alertOffset = 2.0;

        // Act
        var measurement = new Measurement(type, moment, value, alertOffset);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(measurement.Type, Is.EqualTo(type));
            Assert.That(measurement.Moment, Is.EqualTo(moment));
            Assert.That(measurement.Value, Is.EqualTo(value));
            Assert.That(measurement.AlertOffset, Is.EqualTo(alertOffset));
        });
    }

    [Test]
    public void Measurement_WhenCreated_CanHaveNoExpectedValueProperty()
    {
        //Arrange
        const string type = "Temperature";
        var moment = DateTime.Now;
        const double value = 25;
        double? alertOffset = null;

        //Act
        var measurement = new Measurement(type, moment, value, alertOffset);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(measurement.Type, Is.EqualTo(type));
            Assert.That(measurement.Moment, Is.EqualTo(moment));
            Assert.That(measurement.Value, Is.EqualTo(value));
            Assert.That(measurement.AlertOffset, Is.Null);
        });
    }
}
