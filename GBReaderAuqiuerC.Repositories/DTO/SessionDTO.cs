namespace GBReaderAuquierC.Repositories.DTO
{
    public record SessionDTO(Dictionary<string, BookSaveDTO> History);
}