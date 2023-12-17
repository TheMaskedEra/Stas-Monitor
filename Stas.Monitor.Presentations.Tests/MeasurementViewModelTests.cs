using NSubstitute;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations.Tests;

public class MeasurementViewModelTests
{
    [Test]
    public void MeasurementViewModel_Value_ShouldReturnFormattedValue_WhenTypeIsTemperature()
    {
        //Arrange
        var measurement = new Measurement("Temperature", DateTime.Now, 25, null);

        var viewModel = new MeasurementViewModel(measurement);

        //Act
        var formattedValue = viewModel.Value;

        //Assert
        Assert.That(formattedValue, Is.EqualTo("25.0°C"));
    }

    [Test]
    public void MeasurementViewModel_Value_ShouldReturnFormattedValue_WhenTypeIsHumidity()
    {
        //Arrange
        var measurement = new Measurement("Humidity", DateTime.Now, 0.5, null);

        var viewModel = new MeasurementViewModel(measurement);

        //Act
        var formattedValue = viewModel.Value;

        //Assert
        Assert.That(formattedValue, Is.EqualTo("50%"));
    }

    [Test]
    public void MeasurementViewModel_Value_ShouldReturnRawValue_WhenTypeIsUnknown()
    {
        //Arrange
        var measurement = new Measurement("UnknownType", DateTime.Now, 125, null);
        var viewModel = new MeasurementViewModel(measurement);

        //Act
        var formattedValue = viewModel.Value;

        //Assert
        Assert.That(formattedValue, Is.EqualTo("125"));
    }

    [Test]
    public void MeasurementViewModel_Moment_ShouldReturnFormattedMoment()
    {
        //Arrange
        var moment = new DateTime(2023, 1, 1, 12, 30, 0);
        var measurement = new Measurement("Temperature", moment, 25, null);
        var viewModel = new MeasurementViewModel(measurement);

        //Act
        var formattedMoment = viewModel.Moment;

        //Assert
        Assert.That(formattedMoment, Is.EqualTo("Sunday 1 January 2023 12:30:00"));
    }

    [Test]
    public void MeasurementViewModel_ExpectedValue_ShouldReturnCorrectExpectedValue()
    {
        // Arrange
        var measurementWithAlert = new Measurement("Temperature", DateTime.Now, 25.5, 2.0);
        var viewModelWithAlert = new MeasurementViewModel(measurementWithAlert);

        var measurementWithoutAlert = new Measurement("Humidity", DateTime.Now, 50.0, null);
        var viewModelWithoutAlert = new MeasurementViewModel(measurementWithoutAlert);

        // Act
        var expectedValueWithAlert = viewModelWithAlert.ExpectedValue;
        var expectedValueWithoutAlert = viewModelWithoutAlert.ExpectedValue;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expectedValueWithAlert, Is.EqualTo("Expected : 02.0°C"));
            Assert.That(expectedValueWithoutAlert, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void MeasurementViewModel_Background_ShouldReturnCorrectBackground()
    {
        // Arrange
        var measurementWithAlert = new Measurement("Temperature", DateTime.Now, 25.5, 2.0);
        var viewModelWithAlert = new MeasurementViewModel(measurementWithAlert);

        var measurementWithoutAlert = new Measurement("Humidity", DateTime.Now, 50.0, null);
        var viewModelWithoutAlert = new MeasurementViewModel(measurementWithoutAlert);

        // Act
        var backgroundWithAlert = viewModelWithAlert.Background;
        var backgroundWithoutAlert = viewModelWithoutAlert.Background;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(backgroundWithAlert, Is.EqualTo("red"));
            Assert.That(backgroundWithoutAlert, Is.Null);
        });
    }

    [Test]
    public void MeasurementViewModel_Type_ShouldReturnCorrectType()
    {
        // Arrange
        var measurement = new Measurement("Humidity", DateTime.Now, 50.0, null);
        var viewModel = new MeasurementViewModel(measurement);

        // Act
        var type = viewModel.Type;

        // Assert
        Assert.That(type, Is.EqualTo("Humidity"));
    }
}
