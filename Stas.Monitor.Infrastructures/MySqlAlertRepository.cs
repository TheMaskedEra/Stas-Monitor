using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository;

namespace Stas.Monitor.Infrastructures;

public class MySqlAlertRepository : IAlertRepository
{
    private readonly string _connectionString;
    public MySqlAlertRepository(IDictionary<string, string> connectionData)
    {
        var host = connectionData["host"];
        var port = connectionData["port"];
        var database = connectionData["database"];
        var user = connectionData["user"];
        var password = connectionData["password"];
        _connectionString = $"Server={host};Database={database};User={user};Password={password};Port={port};";
    }

    public Alert? RetrieveAlert()
    {
        //TODO
        return null;
    }
}
