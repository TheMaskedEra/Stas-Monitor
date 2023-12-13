using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Presentations;

public class MeasurementPresenter
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

    public  IDictionary<string, IEnumerable<MeasurementViewModel>> RetrieveThermometerMeasurements()
    {
        var viewModels = new Dictionary<string, IEnumerable<MeasurementViewModel>>();

        var measurements = _measurementRepository.GetMeasurementsByThermometer(CurrentThermometer, CurrentTimeSpan);

        var groupedMeasurements = measurements.GroupBy(measurement => measurement.Type);
        foreach (var group in groupedMeasurements)
        {
            var measurementViewModels = group.Select(measurement => new MeasurementViewModel(measurement));

            viewModels.Add(group.Key, measurementViewModels);
        }

        return viewModels;
    }
}
