namespace Stas.Monitor.Domains;

public record Thermometer(string Name) : IComparable
{
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
