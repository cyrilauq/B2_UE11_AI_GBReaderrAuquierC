namespace GBReaderAuquierC.Repositories.DTO
{
    public record SessionDto(Dictionary<string, BookSaveDto> History);
}