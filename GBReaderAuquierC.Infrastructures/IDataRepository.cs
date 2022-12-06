using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Repositories;

public interface IDataRepository
{
    // TODO : Créer base de données MySQL server ==> pour les tests
    public IList<Book> GetBooks();

    public Book Search(string isbn);
    
    // TODO : Créer un méthode loadBook(string isbn) qui chargera toutes les données du livre ayant l'isbn donné.
    public Book LoadBook(string isbn);

    public void SaveSession(Session session);
}