namespace GBReaderAuquierC.Infrastructures.SQL
{
    public static class SqlInstructions
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
        
        public const string GET_ALL_BOOKS = "SELECT b.id_book, b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author " +
                                            "FROM book b " +
                                            "WHERE b.datePublication IS NOT NULL " +
                                            "LIMIT @begin, @end";

        public const string GET_PAGE_FOR_BOOK = "SELECT id_page, content " +
                                                "FROM page " +
                                                "WHERE id_book = @id_book";

        public const string GET_CHOICE_FOR_PAGE = "SELECT p.content, c.content as target_content " +
                                                  "FROM choice p " +
                                                  "JOIN page c ON c.id_page = p.id_target " +
                                                  "WHERE p.id_page = @id_page";
        
        public const string GET_BOOK_BY_ISBN = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                               "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                               "FROM book b " +
                                               "WHERE b.isbn = @isbn && datePublication IS NOT NULL";
    }
}