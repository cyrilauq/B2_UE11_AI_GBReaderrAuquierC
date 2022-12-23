namespace GBReaderAuquierC.Infrastructures.DTO
{
    public record SessionDto(Dictionary<string, BookSaveDto> History);
}