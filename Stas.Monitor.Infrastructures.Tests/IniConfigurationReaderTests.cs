using NUnit.Framework.Constraints;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.Repository.Exceptions;
using Stas.Monitor.Infrastructures.Ini;

namespace Stas.Monitor.Infrastructures.Tests;

public class Tests
{
    private readonly string _filePath = $"{Directory.GetCurrentDirectory()}/resources/configs/config.ini";
    private IniConfigurationReader _reader;

    [SetUp]
    public void SetUp() => _reader = new IniConfigurationReader(_filePath);

    [Test]
    public void Returns_Valid_Configuration()
    {
        var config = _reader.RetrieveConfiguration();

        if (config.Data.Count > 0
            && config.ThermometerNames.Count > 0)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [Test]
    public void Throws_Error_On_File_Not_Found()
    {
        const string badFilePath = "I'm a bad file path.";

        var badReader = new IniConfigurationReader(badFilePath);
        try
        {
            var config = badReader.RetrieveConfiguration();
        }
        catch (ConfigurationReaderException)
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
}
