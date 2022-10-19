namespace GBReaderAuquierC.Presenter;

/// <summary>
/// Contient les informations à donner à une DescriptionBookView
/// </summary>
public record BookItem(string Title,
    string Author,
    string Isbn,
    string ImgPath);