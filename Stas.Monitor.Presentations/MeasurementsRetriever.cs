using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Presentations.IPresenters;

namespace Stas.Monitor.Presentations;

public class MeasurementsRetriever : IMeasurementsRetriever
{
    private readonly IMeasurementRepository _measurementRepository;

    public MeasurementsRetriever(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public IList<MeasurementViewModel> RetrieveMeasurements(string thermometer, string type, double timer)
    {
        var measurements = _measurementRepository.GetMeasurementsByThermometer(thermometer, type, timer);

        return measurements.Select(measurement => new MeasurementViewModel(measurement)).ToList();
    }
}
