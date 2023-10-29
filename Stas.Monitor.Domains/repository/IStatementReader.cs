namespace Stas.Monitor.Domains.repository;

public interface IStatementReader
{
  List<IStatement> GetStatements(Thermometer thermometer);
}