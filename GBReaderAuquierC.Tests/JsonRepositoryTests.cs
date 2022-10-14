using GBReaderAuquierC.Domains.Repository;
using GBReaderAuquierC.Presenter;
using NUnit.Framework.Internal;

namespace GBReaderAuquierC.Tests;

public class JsonRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void throwErrorIfDirectoryDoesNotExist()
    {
        Assert.Throws(typeof(DirectoryNotFoundException),
            () => new JsonRepository().getData(
                Path.Join(
                    Environment.CurrentDirectory.ToString(),
                    "Resources", 
                    "RepositoryNotExistTest").ToString(), "test.json"));
    }

    [Test]
    public void throwErrorIfFileNotFound()
    {
        Assert.Throws(typeof(FileNotFoundException),
            () => new JsonRepository().getData(
                Environment.GetEnvironmentVariable("USERPROFILE").ToString(),
                "test.json"));
    }

    [Test]
    public void Test1()
    {
        if (!File.Exists(
                @"C:\Users\cyril\ue36\AI\GBReaderAuquierC\GBReaderAuquierC.Tests\Resources\FileExists\test.json"))
        {
            File.Create(
                @"C:\Users\cyril\ue36\AI\GBReaderAuquierC\GBReaderAuquierC.Tests\Resources\FileExists\test.json");
        }
        var actual = new JsonRepository().getData(
            @"C:\Users\cyril\ue36\AI\GBReaderAuquierC\GBReaderAuquierC.Tests\Resources\FileExists",
            "test.json");
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
        File.Delete(
            @"C:\Users\cyril\ue36\AI\GBReaderAuquierC\GBReaderAuquierC.Tests\Resources\FileExists\test.json");
    }
}