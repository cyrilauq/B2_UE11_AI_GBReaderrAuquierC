namespace GBReaderAuquierC.Domains.Repository
{
public class BookDTO
{
    private readonly string _title;
    private readonly string _author;
    private readonly string _isbn;
    private readonly string _version;
    private readonly string _resume;
    private readonly string _imagePath;
    
    public string Title { get => _title; }
    public string Author { get => _author; }
    public string ISBN { get => _isbn; }
    public string Resume { get => _resume; }
    public string ImagePath { get => _imagePath; }

    public BookDTO(string title, string resume, string author, string isbn, string imgPath, string version = "1.1")
    {
        _title = title;
        _author = author;
        _resume = resume;
        _isbn = isbn;
        _version = version;
        _imagePath = imgPath;
    }

    public Book ToBook()
    {
        switch(_version)
        {
            case "1.1":
                return FromV1_1();
            case "1":
                return FromV1();
            default:
                return null;
        }
    }
    
    private Book FromV1_1() => new(Title, Author, ISBN, Resume, ImagePath);

    private Book FromV1() => new(Title, Author, ISBN, Resume);
    public override string ToString()
    {
        return $"{Title}, {ISBN}, {Resume}, {Author}";
    }

    public override bool Equals(object? obj)
    {
        if(this == obj) { return true; }
        if(obj == null || !GetType().Equals(obj.GetType())) { return false; }
        BookDTO that = (BookDTO)obj;
        return ISBN.Equals(that.ISBN);
    }
}
}