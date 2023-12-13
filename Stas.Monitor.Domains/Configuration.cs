namespace Stas.Monitor.Domains;

public record Configuration(ISet<string> ThermometerNames,
    IDictionary<string, string> Data
    );
