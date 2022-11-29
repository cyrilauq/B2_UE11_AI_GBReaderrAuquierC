namespace GBReaderAuquierC.Domains;

public class Session
{
    private readonly string _author;

    private string _currentBook;

    public string Autor
    {
        init => _author = value;
        get => _author;
    }

    public string CurrentBook
    {
        set => _currentBook = value;
        get => _currentBook;
    }


}