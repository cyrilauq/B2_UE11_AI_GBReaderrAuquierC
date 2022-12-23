namespace GBReaderAuquierC.Infrastructures.DTO
{
    public record BookSaveDto(string BeginDate, string LastUpdate, IList<int> History);
}