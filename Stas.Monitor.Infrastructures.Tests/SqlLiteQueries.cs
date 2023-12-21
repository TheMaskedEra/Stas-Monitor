using Stas.Monitor.Infrastructures.Database;

namespace Stas.Monitor.Infrastructures.Tests;

public class SqlLiteQueries : IQueries
{
    public string MeasurementSelect => "SELECT m.value, m.type, m.datetime, a.expected_value " +
                                       "FROM Measurements m " +
                                       "LEFT OUTER JOIN Alerts a on m.id = a.measurement_id " +
                                       "WHERE m.room = @thermometer " +
                                       "AND m.datetime BETWEEN" +
                                       "        (SELECT DATETIME(MAX(datetime), '-' || @limit || ' seconds') " +
                                       "        FROM Measurements" +
                                       "        WHERE room = @thermometer)" +
                                       "AND     (SELECT MAX(datetime) " +
                                       "        FROM Measurements" +
                                       "        WHERE room = @thermometer)" +
                                       "ORDER BY m.datetime DESC";
}
