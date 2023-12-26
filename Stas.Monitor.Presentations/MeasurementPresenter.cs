using Stas.Monitor.Presentations.IPresenters;

namespace Stas.Monitor.Presentations;

public class MeasurementPresenter : IMeasurementPresenter
{
    private readonly IMeasurementsRetriever _measurementsRetriever;

    public string CurrentThermometer { get; set; }

    public double CurrentTimer { get; set; }

    public string[] Types { get; init; } = Array.Empty<string>();

    public MeasurementPresenter(IMeasurementsRetriever measurementsRetriever)
    {
        _measurementsRetriever = measurementsRetriever;

        CurrentThermometer = string.Empty;
        CurrentTimer = 0;
    }

    public IDictionary<string, IList<MeasurementViewModel>> RetrieveThermometerMeasurements()
    {
        var measurementViewModels = new Dictionary<string, IList<MeasurementViewModel>>();

        for (var i = 0; i < Types.Length; i++)
        {
            var type = Types[i];
            measurementViewModels.Add(type, _measurementsRetriever.RetrieveMeasurements(CurrentThermometer, type, CurrentTimer));
        }

        return measurementViewModels;
    }
}
