using GBReaderAuquierC.Domains.Repository;
using GBReaderAuquierC.Presenter;
using NUnit.Framework.Internal;

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
                    Environment.CurrentDirectory,
                    "Resources", 
                    "RepositoryNotExistTest"), "test.json").GetData());
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
        try
        {
            using var fs = File.OpenRead(
                Path.Join(_testResourcesPaht, "FileExists", "test.json"));
            fs.Close();
        }
        catch (FileNotFoundException)
        {
            using var create = File.Create(
                Path.Join(_testResourcesPaht, "FileExists", "test.json"));
        }
        var actual = new JsonRepository(
            @"C:\Users\cyril\ue36\AI\GBReaderAuquierC\GBReaderAuquierC.Tests\Resources\FileExists",
            "test.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
        try
        {
            File.Delete(
                Path.Join(_testResourcesPaht, "FileExists", "test.json"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}