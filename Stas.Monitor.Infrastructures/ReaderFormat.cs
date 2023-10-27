namespace Stas.Monitor.Infrastructures;

public static class ReaderFormat
{
  public const string IniFormatRegex = "^\\[thermometers\\]\r\n" + 
                                       @"(t\d\d=(\S| )*\r\n)+"+
                                       "\\[paths\\]\r\n"+
                                       "measures=.*\r\n"+
                                       "alerts=.*$";

  public const string MeasurementCsvFormat = "Name,DateTime,Type,Value";
}