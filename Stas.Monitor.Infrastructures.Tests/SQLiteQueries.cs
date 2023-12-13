namespace Stas.Monitor.Infrastructures.Tests;

public class SQLiteQueries : IQueries
{
    public string SelectType => "SELECT DISTINCT type " +
                                "FROM Measurements " +
                                "WHERE room = @thermometer";

    public string SelectMeasurements => "SELECT m.value, m.datetime, a.expected_value " +
                                        "FROM Measurements m " +
                                        "LEFT OUTER JOIN Alerts a ON m.id = a.measurement_id " +
                                        "WHERE room = @thermometer AND type = @type " +
                                        "ORDER BY datetime DESC";
}
