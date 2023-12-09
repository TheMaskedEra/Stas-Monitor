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
                return new MeasurementTypeDetails("Temperature");
            case MeasurementType.HUMIDITY:
                return new MeasurementTypeDetails("Humidity");
            default:
                throw new ArgumentException("Invalid MeasurementType");
        }
    }
}
