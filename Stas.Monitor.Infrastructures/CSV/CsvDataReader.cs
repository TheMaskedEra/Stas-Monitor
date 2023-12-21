using Serilog;

namespace Stas.Monitor.Infrastructures.CSV;

public class CsvDataReader
{
    private readonly string _filePath;

    public CsvDataReader(string filePath)
    {
        _filePath = filePath;
    }

    public IList<string[]> ReadFile()
    {
        var records = new List<string[]>();

        using var reader = new StreamReader(_filePath);

        var header = reader.ReadLine()?.Split(",");
        if (header == null)
        {
            throw new EndOfStreamException("The File is empty.");
        }

        Header = header!;
        var lineCount = 0;
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

            records.Add(values);
        }

        return records;
    }

    public string[] Header { get; private set; }
}
