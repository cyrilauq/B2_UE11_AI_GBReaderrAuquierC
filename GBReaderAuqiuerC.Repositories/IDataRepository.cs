using GBReaderAuquierC.Domains.Repository;

namespace GBReaderAuquierC.Presenter;

public interface IDataRepository
{
    public void addBook();

    public List<BookDTO> getData(string path, string file);

    public void remove();
}