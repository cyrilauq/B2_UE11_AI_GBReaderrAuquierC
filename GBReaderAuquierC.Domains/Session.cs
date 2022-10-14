namespace GBReaderAuquierC.Domains;

public class Session
{
    private readonly string _author;

    public string Autor
    {
        init => _author = value;
        get => _author;
    }


}