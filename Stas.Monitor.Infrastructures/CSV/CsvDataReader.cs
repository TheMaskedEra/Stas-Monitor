namespace Stas.Monitor.Infrastructures.CSV;

public class CsvDataReader
{
    private readonly string _filePath;

    private string[] Header { get; }

    public string[] Ruler
    {
        get;
        init;
    }

    public CsvDataReader(string filePath)
    {
        _filePath = filePath;

        using var reader = new StreamReader(_filePath);
        var header = reader.ReadLine()?.Split(",");

        Header = header ?? throw new EndOfStreamException();
        Ruler = Array.Empty<string>();
    }

    public IList<string[]> ReadFile()
    {
        var records = new List<string[]>();

        using var reader = new StreamReader(_filePath);
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line!.Split(",");
            if (values.Length != Header.Length)
            {
                continue;
            }

            if (VerifyArguments(values))
            {
                records.Add(values);
            }
        }

        return records;
    }

    private bool VerifyArguments(string[] values)
    {
        if (Ruler.Length == 0)
        {
            return true;
        }

        for (var i = 0; i < values.Length; i++)
        {
            if (Ruler[i] == string.Empty)
            {
                continue;
            }

            if (Ruler[i] != values[i])
            {
                return false;
            }
        }

        return true;
    }
}
