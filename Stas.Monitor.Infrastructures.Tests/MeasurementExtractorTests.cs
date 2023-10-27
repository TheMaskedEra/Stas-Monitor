using Stas.Monitor.Domains.repository;

namespace Stas.Monitor.Infrastructures.Tests;

public class MeasurementExtractorTests
{
  [Test]
  public void Throw_Error_On_Missing_File()
  {
    try
    {
      IMeasurementExtractor extractor = new MeasurementExtractor("");
      Assert.Fail();
    }
    catch (FileNotFoundException)
    {
      Assert.Pass();
    }
  }

  [Test]
  public void Throws_Error_On_Empty_File()
  {
    Assert.Fail();
  }
}