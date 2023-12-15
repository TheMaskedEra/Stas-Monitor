using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Presentations;

public class MeasurementPresenter : IMeasurementPresenter
{
    private readonly IMeasurementRepository _measurementRepository;

    public string CurrentThermometer { get; set; }

    public TimeSpan CurrentTimeSpan { get; set; }

    public MeasurementPresenter(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;

        CurrentThermometer = string.Empty;
        CurrentTimeSpan = TimeSpan.Zero;
    }

    public IEnumerable<MeasurementViewModel> RetrieveThermometerMeasurements()
    {
        var measurements = _measurementRepository.GetMeasurementsByThermometer(CurrentThermometer, CurrentTimeSpan.TotalSeconds);

        var viewModels = measurements.Select(measurement => new MeasurementViewModel(measurement));

        return viewModels;
    }
}
