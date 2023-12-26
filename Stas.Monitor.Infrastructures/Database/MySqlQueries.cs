namespace Stas.Monitor.Infrastructures.Database;

public class MySqlQueries : IQueries
{
    public string MeasurementSelect => "SELECT m.value, m.type, m.datetime, a.expected_value " +
                                        "FROM Measurements m " +
                                        "LEFT OUTER JOIN Alerts a ON m.id = a.measurement_id " +
                                        "WHERE m.type = @type AND m.room = @thermometer AND m.datetime BETWEEN " +
                                        "       (SELECT MAX(datetime) - INTERVAL @limit SECOND " +
                                        "       FROM Measurements" +
                                        "       WHERE room = @thermometer)" +
                                        "AND" +
                                        "       (SELECT MAX(datetime) " +
                                        "      FROM Measurements" +
                                        "       WHERE room = @thermometer)" +
                                        " " +
                                        "ORDER BY datetime DESC";
}
