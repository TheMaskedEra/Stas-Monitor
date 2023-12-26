using NSubstitute;
using NSubstitute.Exceptions;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Presentations.Tests;

public class MeasurementRetrieverTests
{
    [Test]
    public void RetrieveMeasurements_Should_ConvertMeasurementsToViewModels_When_RepositoryReturnsData()
    {
        //Arrange
        const string thermometer = "Bedroom";
        const string type = "Temperature";
        const int timer = 30;

        var measurementFromRepository = new List<Measurement>
        {
            new (type, DateTime.Now, 20, null),
            new (type, DateTime.Now, 19, null)
        };

        var measurementViewModel = new List<MeasurementViewModel>();

        foreach (var measurement in measurementFromRepository)
        {
            measurementViewModel.Add(new MeasurementViewModel(measurement));
        }

        var measurementRepositoryMock = Substitute.For<IMeasurementRepository>();
        measurementRepositoryMock.GetMeasurementsByThermometer(thermometer, type, timer).Returns(measurementFromRepository);

        var measurementRetriever = new MeasurementsRetriever(measurementRepositoryMock);

        //Act
        var result = measurementRetriever.RetrieveMeasurements(thermometer, type, timer);

        //Assert
        Assert.That(result, Has.Count.EqualTo(measurementViewModel.Count));
        CollectionAssert.AreEqual(result, measurementViewModel);
    }
}
