using System.Text.RegularExpressions;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.repository;
using Stas.Monitor.Domains.repository.exceptions;

namespace Stas.Monitor.Infrastructures;

public class MeasurementReader : IStatementReader
{
  private readonly string _filePath;
  public MeasurementReader(string filePath)
  {
    filePath = Directory.GetCurrentDirectory() + "/" + filePath;
    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException("monitor: cannot find measurements file.");
    }

    _filePath = filePath;
  }

  /*
   * Find every measurement from a given thermometer
   */
  public List<IStatement> GetStatements(Thermometer thermometer)
  {
    var name = thermometer.Name;
    var measurements = new List<IStatement>();
    
    try
    {
      using (var reader = new StreamReader(_filePath))
      {
        var format = reader.ReadLine();
        if ( format is null || !Regex.IsMatch(format, ReaderFormat.MeasurementCsvFormat) )
          throw new UnableToExtractDataException("monitor: unable to read measurements file because it is not correctly formatted or empty.");

        while (reader.ReadLine() is { } line)
        {
          var measurement = line.Split(',');
          if (!measurement[0].Equals(name) || measurement.Length != 4 || !measurement[2].Equals("temp"))
            continue;
          
          try
          {
            var moment = DateTime.Parse(measurement[1]);
            measurements.Add(new Measurement(measurement[0], moment, measurement[2], measurement[3]));
          }
          catch (FormatException)
          {
            throw new UnableToExtractDataException("monitor: the DateTime data in the csv file isn't correctly formatted");
          }
        }
      }

      return measurements;
    }
    catch (IOException e)
    {
      throw new UnableToExtractDataException("monitor: something went wrong while parsing measurements : " + e.Message);
    }
  }
}