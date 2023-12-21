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
            //TODO throw exception
        }

        var lineCount = 0;
        while (!reader.EndOfStream)
        {
            lineCount++;
            var line = reader.ReadLine();
            var values = line!.Split(",");
            if (values.Length != header!.Length)
            {
                Log.Logger.Warning("Something is wrong in the " + _filePath + " file: Line " + lineCount + " doesn't have the same amount of variables than the header.");
                continue;
            }

            records.Add(values);
        }

        return records;
    }
}
