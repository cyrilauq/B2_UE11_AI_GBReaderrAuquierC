namespace GBReaderAuquierC.Domains;

public class Book
{
    private readonly string _title;
    private readonly string _author;
    private readonly ISBN _isbn;
    private readonly string _resume;
    private readonly string _imgPath;
    
    public string Title { get => _title; }
    public string Author { get => _author; }
    public String ISBN { get => _isbn.CODE; }
    public string Resume { get => _resume; }
    public string Image { get => _imgPath; }

    public static bool operator true(Book b)
    { 
        if(b.Author == null || b.Author.Trim().Length == 0) { return false; }
        if(b.Title == null || b.Title.Trim().Length == 0) { return false; }
        if(!b._isbn.IsValid()) { return false; }
        if(b.Resume == null || b.Resume.Trim().Length == 0) { return false; }
        return true;
    }

    public static bool operator false(Book b)
    { 
        if(b.Author == null) { return true; }
        if(b.Title == null) { return false; }
        if(b.ISBN == null) { return false; }
        if(b.Resume == null || b.Resume.Trim().Length == 0) { return true; }
        return false;
    }

    public Book(string title, string author, string isbn, string resume)
    {
        _title = title;
        _author = author;
        _isbn = new ISBN(isbn);
        _resume = resume;
    }

    public Book(string title, string author, string isbn, string resume, string imgPath)
        : this(title, author, isbn, resume)
    {
        _imgPath = imgPath;
    }
}