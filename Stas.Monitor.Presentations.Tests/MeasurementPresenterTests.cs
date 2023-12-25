using NSubstitute;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IPresenters;

namespace Stas.Monitor.Presentations.Tests;

public class MeasurementPresenterTests
{
    [Test]
    public void MeasurementPresenter_RetrieveThermometerMeasurements_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var measurementsRetriever = Substitute.For<IMeasurementsRetriever>();
        var measurementPresenter = new MeasurementPresenter(measurementsRetriever) {
            CurrentThermometer = "Living Room", CurrentTimer = 30
        };
        var types = Enum.GetNames(typeof(MeasurementType));

        // Act
        measurementPresenter.RetrieveThermometerMeasurements();

        // Assert
        for (var i = 1; i < types.Length; i++)
        {
            measurementsRetriever.Received(1).RetrieveMeasurements(measurementPresenter.CurrentThermometer, types[i], measurementPresenter.CurrentTimer);
        }
    }
}
