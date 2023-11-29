namespace Stas.Monitor.Domains;

public class Thermometer : IComparable
{
    private readonly string _name;

    public Thermometer(string name)
    {
        _name = name;
    }

    public string Name => _name;

    public override bool Equals(object? obj)
    {
        var thermometer = obj as Thermometer;
        return thermometer != null && Name.Equals(thermometer.Name);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public int CompareTo(object? obj)
    {
        if (obj is Thermometer thermometer)
        {
         return string.CompareOrdinal(this.Name, thermometer.Name);
        }

        throw new ArgumentException("Object must be of type Thermometer", nameof(obj));
  }
}
