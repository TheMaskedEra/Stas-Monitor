using NSubstitute;
using NSubstitute.Exceptions;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IPresenters;

namespace Stas.Monitor.Presentations.Tests;

public class MeasurementPresenterTests
{
    [Test]
    public void RetrieveThermometerMeasurements_ShouldCallRetrieveMeasurementsForEachType()
    {
        //Arrange
        var measurementsRetrieverMock = Substitute.For<IMeasurementsRetriever>();
        var presenter = new MeasurementPresenter(measurementsRetrieverMock)
        {
            Types = new[] { "Temperature", "Humidity" }, CurrentThermometer = "Bedroom", CurrentTimer = 30
        };

        //Act
        var result = presenter.RetrieveThermometerMeasurements();

        //Assert
        foreach (var type in presenter.Types)
        {
            measurementsRetrieverMock.Received(1).RetrieveMeasurements("Bedroom", type, 30);
        }
    }

    [Test]
    public void RetrieveThermometerMeasurements_ShouldReturnDictionaryWithMeasurementViewModels()
    {
        //Arrange
        var measurementsRetrieverMock = Substitute.For<IMeasurementsRetriever>();
        var presenter = new MeasurementPresenter(measurementsRetrieverMock)
        {
            Types = new[] { "Temperature", "Humidity" }, CurrentThermometer = "Bedroom", CurrentTimer = 30
        };

        var expectedMeasurementsTypeTemperature = new List<MeasurementViewModel> { new (new Measurement("Temperature", DateTime.Now, 20, null)), new(new Measurement("Temperature", DateTime.Now, 20, 21)) };
        var expectedMeasurementsTypeHumidity = new List<MeasurementViewModel> { new(new Measurement("Humidity", DateTime.Now, 0.3, null)), new (new Measurement("Humidity", DateTime.Now, 0.3, 0.1)) };

        measurementsRetrieverMock.RetrieveMeasurements("Bedroom", "Temperature", 30).Returns(expectedMeasurementsTypeTemperature);
        measurementsRetrieverMock.RetrieveMeasurements("Bedroom", "Humidity", 30).Returns(expectedMeasurementsTypeHumidity);

        //Act
        var result = presenter.RetrieveThermometerMeasurements();

        // Assert
        Assert.That(result, Is.TypeOf<Dictionary<string, IList<MeasurementViewModel>>>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Keys, Is.EquivalentTo(presenter.Types));
            Assert.That(result["Temperature"], Is.EqualTo(expectedMeasurementsTypeTemperature));
            Assert.That(result["Humidity"], Is.EqualTo(expectedMeasurementsTypeHumidity));
        });
    }

    [Test]
    public void RetrieveThermometerMeasurements_ShouldReturnEmptyDictionaryWhenTypesIsEmpty()
    {
        // Arrange
        var measurementsRetrieverMock = Substitute.For<IMeasurementsRetriever>();
        var presenter = new MeasurementPresenter(measurementsRetrieverMock);

        // Act
        var result = presenter.RetrieveThermometerMeasurements();

        // Assert
        Assert.That(result, Is.TypeOf<Dictionary<string, IList<MeasurementViewModel>>>());
        Assert.That(result, Is.Empty);
    }
}
