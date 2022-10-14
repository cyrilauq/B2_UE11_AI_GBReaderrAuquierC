namespace GBReaderAuquierC.Domains;

public class Book
{
    private readonly string _title;
    private readonly string _author;
    private readonly string _isbn;
    private readonly string _resume;
    private readonly string _imgPath;
    
    public string Title { get => _title; }
    public string Author { get => _author; }
    public string ISBN { get => _isbn; }
    public string Resume { get => _isbn; }
    public string Image { get => _imgPath; }

    public Book(string title, string author, string isbn, string resume)
    {
        _title = title;
        _author = author;
        _isbn = isbn;
        _resume = resume;
    }

    public Book(string title, string author, string isbn, string resume, string imgPath)
        : this(title, author, isbn, resume)
    {
        _imgPath = imgPath;
    }
}