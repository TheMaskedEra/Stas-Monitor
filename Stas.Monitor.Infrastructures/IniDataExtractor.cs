namespace Stas.Monitor.Infrastructures;

public class IniDataExtractor
{
    public IDictionary<string, IDictionary<string, string>> ExtractData(string filePath)
    {
        var iniData = new Dictionary<string, IDictionary<string, string>>();
        string? currentSection = null;

        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var trimmedLine = line.Trim();

                if (ShouldPass(line))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    iniData[currentSection] = new Dictionary<string, string>();
                }
                else if (currentSection != null)
                {
                    PopulateSection(line, iniData, currentSection);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the INI file: {ex.Message}");
        }

        return iniData;
    }

    private bool ShouldPass(string line) => string.IsNullOrWhiteSpace(line) || line.StartsWith(";");

    private void PopulateSection(string line, IDictionary<string, IDictionary<string, string>> iniMap, string section)
    {
        var parts = line.Split('=');
        if (parts.Length != 2)
        {
            return;
        }

        var key = parts[0].Trim();
        var value = parts[1].Trim();
        iniMap[section][key] = value;
    }
}
