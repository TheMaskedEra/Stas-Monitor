using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace Stas.Monitor.Infrastructures.CSV;

public class CsvDataReader : ICsvDataReader //TODO : Implementation qui prend un type en trop
{
    private readonly string _filePath;

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
        var lineCount = 0;
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            lineCount++;
            var line = reader.ReadLine();
            var values = line!.Split(",");
            if (values.Length != Header.Length)
            {
                Log.Logger.Warning("Something is wrong in the " + _filePath + " file: Line " + lineCount + " doesn't have the same amount of variables than the header.");
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

    public string[] Header { get; }

    public string[] Ruler
    {
        get;
        set;
    }
}
