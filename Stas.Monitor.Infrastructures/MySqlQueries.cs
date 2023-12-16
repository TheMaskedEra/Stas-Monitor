namespace Stas.Monitor.Infrastructures;

public class MySqlQueries : IQueries
{
    public string MeasurementSelect => "SELECT m.value, m.type, m.datetime, a.expected_value " +
                                        "FROM Measurements m " +
                                        "LEFT OUTER JOIN Alerts a ON m.id = a.measurement_id " +
                                        "WHERE room = @thermometer AND m.datetime BETWEEN DATE_SUB(NOW(), INTERVAL @limit SECOND) AND NOW() " +
                                        "ORDER BY datetime DESC";
}
