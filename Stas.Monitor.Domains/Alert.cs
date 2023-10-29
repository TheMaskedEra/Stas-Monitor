namespace Stas.Monitor.Domains;

public record Alert(string ThermometerName, DateTime Moment, string ExpectedValue, string ObservedValue) : IStatement
{
  
}