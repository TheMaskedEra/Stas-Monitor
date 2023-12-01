namespace Stas.Monitor.Domains;

public record Measurement(string Name, DateTime Moment, string Type, string Value, Alert? Alert) : IStatement
{
}
