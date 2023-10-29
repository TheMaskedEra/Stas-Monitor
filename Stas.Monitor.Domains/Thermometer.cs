namespace Stas.Monitor.Domains;

public class Thermometer : IComparable
{
  //TODO Quand faut il faire des propriétés des attributs privés ?
  public Thermometer(string name)
  {
    Name = name;
  }

  public string Name { get; }

  public override bool Equals(object? obj)
  {
    var thermometer = obj as Thermometer;
    return thermometer != null && Name.Equals(thermometer.Name);
  }

  public override int GetHashCode()
  {
    return Name.GetHashCode();
  }

  public int CompareTo(object? obj)
  {
    if (obj is Thermometer thermometer)
    {
      return string.CompareOrdinal(this.Name, thermometer.Name);
    }
    throw new ArgumentException("Object must be of type Thermometer", nameof(obj));
  }
}