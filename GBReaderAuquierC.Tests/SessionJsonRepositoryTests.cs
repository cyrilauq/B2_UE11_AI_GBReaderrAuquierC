using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests
{
    public class SessionJsonRepositoryTests
    {
        private readonly string _testResourcesPaht = Path.Join(
            new DirectoryInfo(
                new DirectoryInfo(
                        Directory.GetParent(Path.Join(Environment.CurrentDirectory)).Parent.ToString()).Parent
                    .ToString()).ToString(),
            "Resources", "Session");
        
        [Test]
        public void whenFileContainsNullObjectThenLoadEmptySession()
        {
            ISessionRepository session = null;
            Assert.DoesNotThrow(() 
                => (session = new SessionJsonRepository(_testResourcesPaht, "nullSessionItems.json")).LoadSession());
            Assert.AreEqual(0, session.History.Count);
        }

        [Test]
        public void whenFileDoesntExistThenCreateFileAndGetEmptyReadingSession()
        {
            string file = "notExistingFile.json";
            ISessionRepository session = null;
            Assert.DoesNotThrow(() 
                => (session = new SessionJsonRepository(_testResourcesPaht, file)).LoadSession());
            Assert.Null(session.ReadingBook);
            Assert.Null(session.ReadingPage);
            Assert.Null(session.CurrentBook);
            Assert.NotNull(session.History);
        }
        
        [Test]
        public void whenFileIsWrongFormattedThenDontThrowsAnyKindOfException()
        {
            ISessionRepository session = null;
            Assert.Throws<DataManipulationException>(() 
                => (session = new SessionJsonRepository(_testResourcesPaht, "wrong_formatted.json")).LoadSession());
        }
        
        [Test]
        public void whenFileWithMissingDataThenDontThrowExceptionAndReturnSessionWithGivenDataFromFile()
        {
            Assert.DoesNotThrow(() 
                => new SessionJsonRepository(_testResourcesPaht, "missingData.json"));
        }
        
        [Test]
        public void whenFileContainsOneTimeThenLoadTheSessionWithTheDataFromTheFile()
        {
            ISessionRepository session = null;
            var history = new Dictionary<string, BookSave>()
            {
                { "2200106092", BookSave.Get(DateTime.Parse("17-12-2022 05:10:50"), DateTime.Parse("17-12-2022 05:10:50"), new List<int>()) }
            };
            Assert.DoesNotThrow(() 
                => (session = new SessionJsonRepository(_testResourcesPaht, "oneSessionItem.json")).LoadSession());
            Assert.AreEqual(1, session.History.Count);
            Assert.That(session.History, Is.EquivalentTo(history));
        }
    }
}