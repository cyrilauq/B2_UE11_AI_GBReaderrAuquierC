using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests;

public class JsonRepositoryTests
{
    private readonly string _testResourcesPaht = Path.Join(
        new DirectoryInfo(
            new DirectoryInfo(
                    Directory.GetParent(Path.Join(Environment.CurrentDirectory)).Parent.ToString()).Parent
                .ToString()).ToString(),
        "Resources");

    [Test]
    public void ThrowErrorIfDirectoryDoesNotExist()
    {
        Assert.Throws(typeof(DirectoryNotFoundException),
            () => new JsonRepository(
                Path.Join(
                    _testResourcesPaht, 
                    "RepositoryNotExistTest"), "test.json").GetData());
    }
    
    [Test]
    public void ThrowErrorIfFileDoesNotExist()
    {
        Assert.Throws(typeof(FileNotFoundException),
            () => new JsonRepository(
                _testResourcesPaht, "test.json").GetData());
    }
    
    [Test]
    public void DoesNotThrowExceptionIfFileContainsNullObject()
    {
        var actual = new JsonRepository(
            _testResourcesPaht,
            "nullObjects.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));

    }
    
    [Test]
    public void DoesNotThrowExceptionIfHasWrongFarmattedFile()
    {
        var actual = new JsonRepository(
            _testResourcesPaht,
            "wrongFormatted.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
    }

    [Test]
    public void ThrowErrorIfFileNotFound()
    {
        Assert.Throws(typeof(FileNotFoundException),
            () => new JsonRepository(
                Environment.GetEnvironmentVariable("USERPROFILE"),
                "test.json").GetData());
    }

    [Test]
    public void ReadEmptyFile()
    {
        var actual = new JsonRepository(
            _testResourcesPaht,
            "emptyFile.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
    }
}