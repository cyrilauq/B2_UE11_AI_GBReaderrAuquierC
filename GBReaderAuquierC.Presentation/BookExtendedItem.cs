namespace GBReaderAuquierC.Presenter;

/// <summary>
/// Contient les informations à donner à une ExtendedDescriptionBookView
/// </summary>
public record BookExtendedItem(string Title,
    string Author,
    string Isbn,
    string ImgPath,
    string Resume);