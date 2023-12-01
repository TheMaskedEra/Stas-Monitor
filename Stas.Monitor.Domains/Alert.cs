namespace Stas.Monitor.Domains;

public record Alert(DateTime Moment, string ExpectedValue) : IStatement
{
}
