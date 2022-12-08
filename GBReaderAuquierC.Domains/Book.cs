namespace GBReaderAuquierC.Domains;

public class Book
{
    private readonly string _title;
    private readonly string _author;
    private readonly ISBN _isbn;
    private readonly string _resume;
    private readonly string _imgPath;

    private IList<Page> _pages = new List<Page>();
    
    public string Title { get => _title; }
    public string Author { get => _author; }
    public String ISBN { get => _isbn.CODE; }
    public string Resume { get => _resume; }
    public string Image { get => _imgPath; }
    
    public IList<Page> Pages { set => _pages = value; }

    public int CountPage { get => _pages.Count; }

    public Page First
    {
        get => _pages.Count == 0 ? null : _pages.First();
    }

    public Page this[int key]
    {
        get => _pages[key];
    }

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

    public Page GetPageFor(string content)
    {
        return _pages.Where(p => p.Content.Equals(content)).First();
    }

    public int GetNPageFor(Page page)
    {
        return _pages.IndexOf(page) + 1;
    }

    public bool IsLastPage(Page page)
        => _pages.Last().Equals(page);

    public override bool Equals(object? obj)
    {
        if(this == obj) { return true; }
        if(this.GetType() != obj.GetType()) { return false; }
        Book that = obj as Book;
        return this.ISBN.Equals(that.ISBN) && this._pages.SequenceEqual(that._pages);
    }

    public override string ToString() => $"{nameof(_title)}: {_title}, " +
                                         $"{nameof(_author)}: {_author}, " +
                                         $"{nameof(_isbn)}: {_isbn}, " +
                                         $"{nameof(_resume)}: {_resume}, " +
                                         $"{nameof(_imgPath)}: {_imgPath}, " +
                                         $"{nameof(_pages)}: {_pages}, " +
                                         $"Page count = {_pages.Count}";
}