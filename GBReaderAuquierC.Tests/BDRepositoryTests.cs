using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests
{
    public class BDRepositoryTests
    {
        private IDataRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo  = new BdRepository("MySql.Data",
                new DbInformations("DESKTOP-TCUHOLK", "gbreader", "cyril", "OUI"));
        }

        [Test]
        public void WhenTryToGetBookWithCorrectConnectionStringThenDoesNotThrowsException()
        {
            Assert.DoesNotThrow(() => _repo.GetBooks());
        }

        [Test]
        public void WhenDbNotAccessibleThenThrowsDataManipulationException()
        {
            Assert.Throws<DataManipulationException>(() => _repo.GetBooks());
        }
    }
}