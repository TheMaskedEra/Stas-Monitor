using NUnit.Framework.Constraints;
using Stas.Monitor.Domains;
using Stas.Monitor.Domains.exceptions;

namespace Stas.Monitor.Infrastructures.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Returns_Configuration_On_Good_Run()
    {
      const string fileName = "configs/config.ini";
      IConfigurationReader reader = new IniConfigurationReader(fileName);
      try
      {
        var config = reader.GetData();
        Assert.That(config, Is.Not.Null);
      }
      catch ( UnableToExtractDataException e )
      {
        Assert.Fail("Expected no exception thrown.");
      }
    }

    [Test]
    public void Throws_Error_On_Missing_Arguments()
    {
      const string fileName = "configs/MissingArgument.ini";
      try
      {
        IConfigurationReader reader = new IniConfigurationReader(fileName);
        var config = reader.GetData();
        Assert.Fail("Expected exception: UnableToExtractData (missing element)");
      }
      catch (UnableToExtractDataException e)
      {
        Assert.Pass();
      }
    }

    [Test]
    public void Throws_Error_On_File_Not_Found()
    {
      const string fileName = "configs/nonExistent.ini";
      try
      {
        IConfigurationReader reader = new IniConfigurationReader(fileName);
        Assert.Fail("Expected thrown exception: FileNotFound");
      }
      catch ( FileNotFoundException e )
      {
        Assert.Pass();
      } 
    }
}