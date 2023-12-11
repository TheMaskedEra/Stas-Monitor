namespace Stas.Monitor.Domains;

public record Measurement(string Type, DateTime Moment, double Value, double? AlertOffset);
