using System.Data;
using System.Data.Common;

namespace Stas.Monitor.Infrastructures.Database;

public class DatabaseRequestsBuilder //TODO peut être retirér
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;

    private DbConnection Connection { get; set; }

    private DbCommand? Command { get; set; }

    public DbDataReader? Reader { get; private set; }

    public DatabaseRequestsBuilder(DbProviderFactory factory, string connectionString)
    {
        _factory = factory;
        _connectionString = connectionString;

        Connection = factory.CreateConnection() ?? throw new InvalidOperationException(); //TODO fix this
        Connection.ConnectionString = connectionString;
        TestConnection();
    }

    private void TestConnection()
    {
        using (Connection)
        {
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something");
            }
        }
    }

    public void CreateConnection()
    {
        Connection.Dispose();
        Connection = _factory.CreateConnection()!;
        Connection.ConnectionString = _connectionString;
    }

    public void CreateCommand(string query)
    {
        Command?.Dispose();
        Command = Connection.CreateCommand();
        Command.CommandText = query;
    }

    public void AddParameter(string name, object value, DbType type)
    {
        if (Command == null)
        {
            return; // TODO Renvoyer une erreur ?
        }

        var parameter = Command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        parameter.DbType = type;
        Command.Parameters.Add(parameter);
    }

    public void Reset()
    {
        Connection.Dispose();
    }

    public void Execute()
    {

    }

    //TODO GetResults()
}
