namespace Stas.Monitor.Domains;

public record Measurement(DateTime Moment, double Value, Alert? Alert);
