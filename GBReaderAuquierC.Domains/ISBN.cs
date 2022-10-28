namespace GBReaderAuquierC.Domains;

public class ISBN
{
    private readonly string _isbn;
    public static readonly string AUTHOR_MATRICULE = "200106";

    public string CODE
    {
        get
        {
            if(_isbn.Length == 11) {
                if(_isbn.EndsWith("10")) {
                    return _isbn.Substring(0, 9) + "X";
                }
                return _isbn.Substring(0, 9) + "0";
            }
            return _isbn;
        }
    }

    public ISBN(string isbn)
    {
        _isbn = isbn;
    }

    public bool IsValid()
    {
        if(CODE == null || CODE.Trim().Length == 0) { return false; }
        if(CODE.Replace("-", "").Length != 11 && CODE.Replace("-", "").Length != 10) { return false; }
        return _isbn.Contains(AUTHOR_MATRICULE);
    }
}