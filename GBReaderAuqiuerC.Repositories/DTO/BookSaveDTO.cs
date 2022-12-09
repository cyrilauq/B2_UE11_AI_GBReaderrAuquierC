namespace GBReaderAuquierC.Repositories.DTO
{
    public record BookSaveDTO(string BeginDate, string LastUpdate, IList<int> History);
}