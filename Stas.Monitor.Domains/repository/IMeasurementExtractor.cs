namespace Stas.Monitor.Domains.repository;

public interface IMeasurementExtractor
{
  List<Measurement> GetMeasurements(Thermometer thermometer);
}