using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests
{
    public class BDRepositoryTests
    {
        private IDataRepository _repo = new BDRepository("MySql.Data.MySqlClient",
            new DbInformations("192.168.128.13", "in20b1001", "in20b1001", "4918"));

        [Test]
        public void WhenTryToGetBookWithCorrectConnectionStringThenDoesNotThrowsException()
        {
            Assert.DoesNotThrow(() => _repo.GetBooks());
        }
    }
}