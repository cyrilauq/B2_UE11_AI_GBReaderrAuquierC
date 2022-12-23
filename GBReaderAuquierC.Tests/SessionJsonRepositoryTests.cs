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
                => new SessionJsonRepository(_testResourcesPaht, "missingData.json").LoadSession());
        }
        
        [Test]
        public void whenFileContainsOneTimeThenLoadTheSessionWithTheDataFromTheFile()
        {
            ISessionRepository session = null;
            var history = new Dictionary<string, ReadingSession>()
            {
                { "2200106092", ReadingSession.Get(DateTime.Parse("17-12-2022 05:10:50"), DateTime.Parse("17-12-2022 05:10:50"), new List<int>()) }
            };
            Assert.DoesNotThrow(() 
                => (session = new SessionJsonRepository(_testResourcesPaht, "oneSessionItem.json")).LoadSession());
            Assert.AreEqual(1, session.History.Count);
            Assert.That(session.History, Is.EquivalentTo(history));
        }

        [Test]
        public void whenSaveSessionWithANonExistingFileTheSaveSessionAndCreateFile()
        {
            var fileName = "nonExistingFile.json";
            ISessionRepository session = new SessionJsonRepository(_testResourcesPaht, fileName);
            var page2 = new Page("Test");
            session.ReadingBook = new Book("Test", "You", "2200106092", "Je me meurs.")
            {
                Pages = new List<Page>()
                {
                    new ("Test")
                    {
                        Choices = new Dictionary<string, Page>()
                        {
                            { "Mange", page2 }
                        }
                    },
                    page2
                }
            };
            var firstSession = session.History.First().Value;
            var history = new Dictionary<string, ReadingSession>()
            {
                { "2200106092", ReadingSession.Get(firstSession.Begin, firstSession.Begin, new List<int>() { 1 }) }
            };
            Assert.DoesNotThrow(() => session.SaveSession());
            Assert.That(session.History.Count, Is.EqualTo(1));
            Assert.That(session.History, Is.EquivalentTo(history));
            File.Delete(Path.Join(_testResourcesPaht, fileName));
        }

        [Test]
        public void whenSaveSessionWithAExistingFileTheSaveSession()
        {
            var fileName = "emptyFile.json";
            ISessionRepository session = new SessionJsonRepository(_testResourcesPaht, fileName);
            var page2 = new Page("Test");
            session.ReadingBook = new Book("Test", "You", "2200106092", "Je me meurs.")
            {
                Pages = new List<Page>()
                {
                    new ("Test")
                    {
                        Choices = new Dictionary<string, Page>()
                        {
                            { "Mange", page2 }
                        }
                    },
                    page2
                }
            };
            var firstSession = session.History.First().Value;
            var history = new Dictionary<string, ReadingSession>()
            {
                { "2200106092", ReadingSession.Get(firstSession.Begin, firstSession.Begin, new List<int>() { 1 }) }
            };
            Assert.DoesNotThrow(() => session.SaveSession());
            Assert.That(session.History.Count, Is.EqualTo(1));
            Assert.That(session.History, Is.EquivalentTo(history));
        }

        [Test]
        public void whenCurrentPageIsTerminalThenTheBookIsDeletedFromTheSession()
        {
            var fileName = "emptyFile.json";
            ISessionRepository session = new SessionJsonRepository(_testResourcesPaht, fileName);
            var page2 = new Page("Test");
            session.ReadingBook = new Book("Test", "You", "2200106092", "Je me meurs.")
            {
                Pages = new List<Page>()
                {
                    new ("Test")
                    {
                        Choices = new Dictionary<string, Page>()
                        {
                            { "Mange", page2 }
                        }
                    },
                    page2
                }
            };
            session.ReadingPage = page2;
            session.ReadingPage = null;
            Assert.DoesNotThrow(() => session.SaveSession());
            Assert.That(session.History.Count, Is.EqualTo(0));
            Assert.That(session.ReadingPage.Content, Is.EqualTo("Test"));
        }

        [Test]
        public void whenAddedBookIsNullThenDoNotAddIt()
        {
            ISessionRepository session = new SessionJsonRepository(_testResourcesPaht, "");
            var page2 = new Page("Test");
            session.ReadingBook = new Book("Test", "You", "2200106092", "Je me meurs.")
            {
                Pages = new List<Page>()
                {
                    new ("Test")
                    {
                        Choices = new Dictionary<string, Page>()
                        {
                            { "Mange", page2 }
                        }
                    },
                    page2
                }
            };
            session.ReadingBook = null;
            Assert.That(session.History.Count, Is.EqualTo(1));
        }

        [Test]
        public void whenAddedCurrentBookIsNullThenCurrentBookIsNotModified()
        {
            ISessionRepository session = new SessionJsonRepository(_testResourcesPaht, "");
            var page2 = new Page("Test");
            session.CurrentBook = new Book("Test", "You", "2200106092", "Je me meurs.")
            {
                Pages = new List<Page>()
                {
                    new ("Test")
                    {
                        Choices = new Dictionary<string, Page>()
                        {
                            { "Mange", page2 }
                        }
                    },
                    page2
                }
            };
            session.CurrentBook = null;
            Assert.That(session.CurrentBook[BookAttribute.Isbn], Is.EqualTo("2200106092"));
        }
    }
}