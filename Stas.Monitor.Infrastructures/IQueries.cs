namespace Stas.Monitor.Infrastructures;

public interface IQueries
{
    string SelectType { get; }

    string SelectMeasurements { get; }
}
