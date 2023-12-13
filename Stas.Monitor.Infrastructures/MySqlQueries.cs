namespace Stas.Monitor.Infrastructures;

public class MySqlQueries : IQueries
{
    public string SelectType => "SELECT DISTINCT type " +
                                "FROM Measurements " +
                                "WHERE room = @thermometer";

    public string SelectMeasurements => "SELECT m.value, m.datetime, a.expected_value " +
                                        "FROM Measurements m " +
                                        "LEFT jOIN Alerts a ON m.id = a.measurement_id " +
                                        "WHERE room = @thermometer AND type = @type " + //AND m.datetime >= NOW() - INTERVAL @limit SECOND " +
                                        "ORDER BY datetime DESC";
}
