namespace GBReaderAuquierC.Repositories.DTO
{
    public record SessionDTO(Dictionary<string, IList<int>> History);
}