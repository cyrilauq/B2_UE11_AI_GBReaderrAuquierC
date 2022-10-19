namespace GBReaderAuquierC.Domains;

public class ISBN
{
    private readonly string _isbn;
    
    public string CODE { get => _isbn; }

    public ISBN(string isbn)
    {
        _isbn = isbn;
    }

    public bool IsValid()
    {
        if(CODE == null || CODE.Trim().Length == 0) { return false; }
        if(CODE.Length != 11 && CODE.Length != 10) { return false; }
        return true;
    }
}