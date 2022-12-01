using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Repositories;

public interface IDataRepository
{
    public IList<Book> GetBooks();

    public Book Search(string isbn);
}