namespace GBReaderAuquierC.Repositories.SQL
{
    public class SqlInstructions
    {
        public const string SEARCH_BOOKS_WITH_BOTH_FILTER = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                                            "FROM book b " +
                                                            "WHERE (b.isbn LIKE @search OR b.title LIKE @search) AND b.datePublication IS NOT NULL " +
                                                            "LIMIT @begin, @end";
        
        public const string SEARCH_BOOKS_WITH_ISBN_FILTER = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                                            "FROM book b " +
                                                            "WHERE b.isbn LIKE @search AND b.datePublication IS NOT NULL " +
                                                            "LIMIT @begin, @end";
        
        public const string SEARCH_BOOKS_WITH_TITLE_FILTER = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                                             "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                                             "FROM book b " +
                                                             "WHERE b.title LIKE @search AND b.datePublication IS NOT NULL " +
                                                             "LIMIT @begin, @end";
    }
}