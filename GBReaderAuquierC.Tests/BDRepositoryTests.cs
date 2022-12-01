using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests
{
    public class BDRepositoryTests
    {
        private IDataRepository _repo = new BDRepository("MySql.Data.MySqlClient", "server=192.168.128.13;database=in20b1001;uid=in20b1001;pwd=4918");

        [Test]
        public void WhenTryToGetBookWithCorrectConnectionStringThenDoesNotThrowsException()
        {
            Assert.DoesNotThrow(() => _repo.GetBooks());
        }
    }
}