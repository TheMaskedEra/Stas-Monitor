namespace Stas.Monitor.Infrastructures.CSV;

public interface ICsvDataReader
{
    public string[] Header { get; }

    public string[] Ruler { get; set; }

    public IList<string[]> ReadFile();
}
