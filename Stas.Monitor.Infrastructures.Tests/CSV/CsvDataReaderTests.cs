using System.Linq.Expressions;
using Stas.Monitor.Infrastructures.CSV;
using Directory = System.IO.Directory;

namespace Stas.Monitor.Infrastructures.Tests.CSV;

public class CsvDataReaderTests
{
    [Test]
    public void CsvDataReader_Should_ThrowErrorOnEmptyFile()
    {
        //Arrange
        var filePath = $"{Directory.GetCurrentDirectory()}/resources/CSV/EmptyFile.csv";
        var csvReader = new CsvDataReader(filePath);

        //Act & Assert
        Assert.Throws<EndOfStreamException>(() => csvReader.ReadFile());
    }

    [Test]
    public void CsvDataReader_Should_ThrowError_OnFileNotFound()
    {
        //Arrange
        var csvReader = new CsvDataReader("A Bad Path");

        //Act & Assert
        Assert.Throws<FileNotFoundException>(() => csvReader.ReadFile());
    }

    [Test]
    public void CsvDataReader_WhenReadFile_Should_IgnoreLinesOfDifferentSizeThanHeader()
    {
        //Arrange
        var filePath = $"{Directory.GetCurrentDirectory()}/resources/CSV/MissingValues.csv";
        var reader = new CsvDataReader(filePath);

        //Act
        var values = reader.ReadFile();

        //Assert
        Assert.That(values, Has.Count.EqualTo(2));
    }

    [Test]
    public void CsvDataReader_WhenReadFile_Should_ReturnListOfStringArrayOfContent()
    {
        //Arrange
        var filePath = $"{Directory.GetCurrentDirectory()}/resources/CSV/MissingValues.csv";
        var reader = new CsvDataReader(filePath);
        var expectedValues = new List<string[]>{ new [] { "Sarah", "18", "Banker" }, new [] { "John", "21", "Scientific" } };

        //Act
        var values = reader.ReadFile();

        //Assert
        Assert.That(values, Is.EqualTo(expectedValues));
    }

    [Test]
    public void CsvDataReader_WhenReadFile_Should_ReturnEmptyListOnEmptyFileWithHeader()
    {
        //Arrange
        var filePath = $"{Directory.GetCurrentDirectory()}/resources/CSV/EmptyValues.csv";
        var reader = new CsvDataReader(filePath);

        //Act
        var values = reader.ReadFile();

        //Assert
        Assert.That(values, Is.Empty);
    }
}
