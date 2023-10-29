namespace Stas.Monitor.Domains.repository;

public interface IStatementReader
{
  IEnumerable<IStatement> GetStatements(Thermometer thermometer);
}