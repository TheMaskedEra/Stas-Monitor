namespace Stas.Monitor.Domains;

public record Alert(double ExpectedValue, string Message) : IStatement
{
}
