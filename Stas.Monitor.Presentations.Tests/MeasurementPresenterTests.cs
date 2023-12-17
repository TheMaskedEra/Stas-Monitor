using NSubstitute;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Presentations.Tests;

public class MeasurementPresenterTests
{
    [Test]
    public void MeasurementPresenter_RetrieveThermometerMeasurements_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var measurementRepositoryMock = Substitute.For<IMeasurementRepository>();
        var measurementPresenter = new MeasurementPresenter(measurementRepositoryMock) {
            CurrentThermometer = "Living Room", CurrentTimeSpan = TimeSpan.FromMinutes(30)
        };

        // Act
        measurementPresenter.RetrieveThermometerMeasurements();

        // Assert
        measurementRepositoryMock.Received(1).GetMeasurementsByThermometer(measurementPresenter.CurrentThermometer, measurementPresenter.CurrentTimeSpan.TotalSeconds);
    }
}
