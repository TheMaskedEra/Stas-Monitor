using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class StatementViewModel
{
  private readonly IStatement _statement;
  public StatementViewModel(IStatement statement)
  {
    _statement = statement;
  }

  public string Value
  {
    get
    {
        if (_statement is Measurement measurement)
        {
         return measurement.Value + "°";
        }

      return (( Alert ) _statement ).ObservedValue + "°";
    }
  }

  public string Moment
  {
    get
    {
        if (_statement is Measurement measurement)
        {
            return measurement.Moment.ToString("dddd d MMMM yyyy hh:mm:ss");
        }

        return ( ( Alert )_statement ).Moment.ToString("dddd d MMMM yyyy hh:mm:ss");
    }
  }

  public string ExpectedValue
  {
    get
    {
      if ( _statement is Alert alert )
      {
        return "Expected : " + alert.ExpectedValue;
      }

      return string.Empty;
    }
  }

  public string Background
  {
    get
    {
      if ( _statement is Alert alert )
      {
        return "CadetBlue";
      }

      return "Black";
    }
  }
}
