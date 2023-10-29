using System.Text.RegularExpressions;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.repository;
using Stas.Monitor.Domains.repository.exceptions;

namespace Stas.Monitor.Infrastructures;

public class AlertReader : IStatementReader
{
  private readonly string _filePath;
  
  public AlertReader(string filePath)
  {
    filePath = Directory.GetCurrentDirectory() + "/" + filePath;
    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException("monitor: cannot find measurements file");
    }

    _filePath = filePath;
  }

  public IEnumerable<IStatement> GetStatements(Thermometer thermometer)
  {
    var name = thermometer.Name;
    var alerts = new List<IStatement>();

    try
    {
      using (var reader = new StreamReader(_filePath))
      {
        var format = reader.ReadLine();
        if ( format is null || !Regex.IsMatch(format, ReaderFormat.AlertCsvFormat) )
          throw new UnableToReadAlertsException("monitor: unable to read alerts file because it  not correctly formatted or empty.");

        while ( reader.ReadLine() is { } line )
        {
          var alert = line.Split(',');
          if ( !alert[0].Equals(name) || alert.Length != 4 )
            continue;
          try
          {
            var moment = DateTime.Parse(alert[1]);
            alerts.Add(new Alert(alert[0], moment, alert[2], alert[3]));
          }
          catch (FormatException)
          {
            throw new UnableToReadAlertsException(
              "monitor: the DateTime data in the csv file isn't correctly formatted.");
          }
         
        }
      }
      return alerts;
    }
    catch (IOException e)
    {
      throw new UnableToReadAlertsException("monitor: something went wrong while parsing alerts : " + e.Message);
    }
  }
}