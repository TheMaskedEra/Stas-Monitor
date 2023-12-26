using System.Globalization;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Infrastructures.CSV;

public class CsvMeasurementRepository : IMeasurementRepository
{
    private readonly string _alertsFilePath;
    private readonly string _measurementsFilePath;

    public CsvMeasurementRepository(string measurementsFilePath, string alertsFilePath)
    {
        _measurementsFilePath = measurementsFilePath;
        _alertsFilePath = alertsFilePath;
    }

    public IList<Measurement> GetMeasurementsByThermometer(string thermometer, string type, double timer)
    {
        var measurementsReader = new CsvDataReader(_measurementsFilePath)
        {
            Ruler = new [] { thermometer, string.Empty, type, string.Empty },
        };
        var alertsReader = new CsvDataReader(_alertsFilePath)
        {
            Ruler = new [] { thermometer, string.Empty, string.Empty, string.Empty },
        };

        var measurementsValues = measurementsReader.ReadFile();
        var alertsValues = alertsReader.ReadFile();

        var parsedMeasurementValues = ParseMeasurementsByTimers(measurementsValues, timer);
        var measurements = CreateMeasurements(parsedMeasurementValues, alertsValues);

        return measurements;
    }

    private IList<string[]> ParseMeasurementsByTimers(IList<string[]> measurementsValues, double timer)
    {
        var parsedMeasurements = new List<string[]>();
        var lastDate = DateTime.Parse(measurementsValues[0][1]);
        parsedMeasurements.Add(measurementsValues[0]);
        var offsetTimer = lastDate.AddSeconds(-timer);

        for (var i = 1; i < measurementsValues.Count; i++)
        {
            var currentMeasurementDatetime = measurementsValues[i][1];
            var currentDatetime = DateTime.Parse(currentMeasurementDatetime);
            if (currentDatetime < offsetTimer)
            {
                break;
            }

            parsedMeasurements.Add(measurementsValues[i]);
        }

        return parsedMeasurements;
    }

    private IList<Measurement> CreateMeasurements(IList<string[]> parsedMeasurementsValues, IList<string[]> alertsValues)
    {
        var measurements = new List<Measurement>();

        foreach (var currentMeasurementValues in parsedMeasurementsValues)
        {
            var alertIndex = FindCompatibleAlert(currentMeasurementValues[1], alertsValues);
            double? alertValue = null;
            if (alertIndex != -1)
            {
                var stringValue = alertsValues[alertIndex][2];
                alertValue = double.Parse(stringValue);
            }

            var datetime = DateTime.Parse(currentMeasurementValues[1]);
            var value = double.Parse(currentMeasurementValues[3]);
            measurements.Add(new Measurement(currentMeasurementValues[2], datetime, value, alertValue));
        }

        return measurements;
    }

    private int FindCompatibleAlert(string datetime, IList<string[]> alertsValues)
    {
        for (var i = 0; i < alertsValues.Count; i++)
        {
            if (alertsValues[i][1].Equals(datetime))
            {
                return i;
            }
        }

        return -1;
    }
}
