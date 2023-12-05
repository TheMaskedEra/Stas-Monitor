namespace Stas.Monitor.Domains;

//TODO Faire comme ceci ?
public enum MeasurementType
{
    TEMPERATURE,
    HUMIDITY
}

public class MeasurementTypeDetails
{
    public string Name { get; }

    private MeasurementTypeDetails(string name)
    {
        Name = name;
    }

    public static MeasurementTypeDetails GetDetails(MeasurementType type)
    {
        switch (type)
        {
            case MeasurementType.TEMPERATURE:
                return new MeasurementTypeDetails("temperature");
            case MeasurementType.HUMIDITY:
                return new MeasurementTypeDetails("humidity");
            default:
                throw new ArgumentException("Invalid MeasurementType");
        }
    }
}
