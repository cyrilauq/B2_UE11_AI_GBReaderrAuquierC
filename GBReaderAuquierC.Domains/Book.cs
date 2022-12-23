namespace GBReaderAuquierC.Domains;

public class Book
{
    private readonly string _imgPath;

    private IList<Page> _pages = new List<Page>();

    private readonly BookMetaData _data;

    public IList<Page> Pages { set => _pages = value; }

    public int CountPage { get => _pages.Count; }

    public Page First
    {
        get => _pages.Count == 0 ? null : _pages.First();
    }

    public string this[BookAttribute attr]
    {
        get => BookAttribute.Image.Equals(attr) ? _imgPath : _data[attr];
    }

    public Page this[int key]
    {
        get => _pages[key];
    }

    public static bool operator true(Book b)
    { 
        if(b[BookAttribute.Author] == null || b[BookAttribute.Author].Trim().Length == 0) { return false; }
        if(b[BookAttribute.Title] == null || b[BookAttribute.Author].Trim().Length == 0) { return false; }
        if(!Isbn.IsValid(b[BookAttribute.Isbn])) { return false; }
        if(b[BookAttribute.Resume] == null || b[BookAttribute.Resume].Trim().Length == 0) { return false; }
        return true;
    }

    public static bool operator false(Book b)
    { 
        if(b[BookAttribute.Author] == null) { return true; }
        if(b[BookAttribute.Title] == null) { return false; }
        if(b[BookAttribute.Isbn] == null) { return false; }
        if(Isbn.IsValid(b[BookAttribute.Isbn])) { return false; }
        if(b[BookAttribute.Resume] == null || b[BookAttribute.Resume].Trim().Length == 0) { return true; }
        return false;
    }
    
    public Book(string title, string author, string isbn, string resume, string imgPath = "")
    {
        _data = new BookMetaData(title, resume, isbn, author);
        _imgPath = imgPath;
    }

    public int GetNPageFor(Page page)
        => _pages.IndexOf(page) + 1;
}