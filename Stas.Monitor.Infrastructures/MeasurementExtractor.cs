using System.Text.RegularExpressions;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.exceptions;
using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Infrastructures;

public class MeasurementExtractor : IMeasurementExtractor
{
  private readonly string _filePath;
  public MeasurementExtractor(string filePath)
  {
    filePath = Directory.GetCurrentDirectory() + "/" + filePath;
    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException("monitor: cannot find measurements file.");
    }

    _filePath = filePath;
  }

  public List<Measurement> GetMeasurements(Thermometer thermometer)
  {
    var name = thermometer.Name;
    var measurements = new List<Measurement>();
    
    try
    {
      using (var reader = new StreamReader(_filePath))
      {
        var format = reader.ReadLine();
        if ( format is null || !Regex.IsMatch(format, ReaderFormat.MeasurementCsvFormat) )
          throw new UnableToExtractDataException("monitor: impossible to read measurements file because it is not correctly formatted.");

        while (reader.ReadLine() is { } line)
        {
          var measurement = line.Split(',');
          if (!measurement[0].Equals(name))
            continue;
          
          measurements.Add(new Measurement(measurement[0], measurement[1], measurement[2], measurement[4]));
        }
      }

      return measurements;
    }
    catch (IOException e)
    {
      throw new UnableToExtractDataException("monitor something went wrong while parsing measurements : " + e.Message);
    }
  }
}