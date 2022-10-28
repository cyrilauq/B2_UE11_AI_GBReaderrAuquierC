using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Repositories;

public interface IDataRepository
{
    public List<BookDTO> GetData();

    public Book Search(string isbn);
}