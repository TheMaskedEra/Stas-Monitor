namespace Stas.Monitor.Domains;

public class Thermometer
{
  public Thermometer(string name)
  {
    Name = name;
  }

  public string Name
  {
    get;
  }

  public override bool Equals(object? obj)
  {
    var thermometer = obj as Thermometer;
    return thermometer != null && Name.Equals(thermometer.Name);
  }

  public override int GetHashCode()
  {
    return Name.GetHashCode();
  }
}