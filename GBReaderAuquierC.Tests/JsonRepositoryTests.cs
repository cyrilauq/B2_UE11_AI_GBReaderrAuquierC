using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests;

public class JsonRepositoryTests
{
    [Test]
    public void ThrowErrorIfDirectoryDoesNotExist()
    {
        Assert.Throws(typeof(DirectoryNotFoundException),
            () => new JsonRepository(
                Path.Join(
                    "Resources", 
                    "RepositoryNotExistTest"), "test.json").GetData());
    }
    
    [Test]
    public void ThrowErrorIfFileDoesNotExist()
    {
        Assert.Throws(typeof(FileNotFoundException),
            () => new JsonRepository("Resources", "test.json").GetData());
    }
    
    [Test]
    public void DoesNotThrowExceptionIfFileContainsNullObject()
    {
        try
        {
            using var fs = File.OpenRead(
                Path.Join("Resources", "nullObjects.json"));
            fs.Close();
        }
        catch (FileNotFoundException)
        {
            using var create = File.Create(
                Path.Join("Resources", "nullObjects.json"));
            using var write = File.AppendText(
                Path.Join("Resources", "nullObjects.json"));
            write.Write("[,]");
        }
        var actual = new JsonRepository(
            "Resources",
            "nullObjects.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
    }
    
    [Test]
    public void DoesNotThrowExceptionIfHasWrongFarmattedFile()
    {
        try
        {
            using var fs = File.OpenRead(
                Path.Join("Resources", "wrongFormatted.json"));
            fs.Close();
        }
        catch (FileNotFoundException)
        {
            using var create = File.Create(
                Path.Join("Resources", "wrongFormatted.json"));
            using var write = File.AppendText(
                Path.Join("Resources", "wrongFormatted.json"));
            write.Write("[,{]");
        }
        var actual = new JsonRepository(
            "Resources",
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
        try
        {
            using var fs = File.OpenRead(
                Path.Join("Resources", "emptyFile.json"));
            fs.Close();
        }
        catch (FileNotFoundException)
        {
            using var create = File.Create(
                Path.Join("Resources", "emptyFile.json"));
        }
        var actual = new JsonRepository(
            @"Resources",
            "emptyFile.json").GetData();
        Assert.That(actual, Is.EqualTo(new List<BookDTO>()));
    }
}