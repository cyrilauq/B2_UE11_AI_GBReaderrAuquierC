using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;
using Newtonsoft.Json;

namespace GBReaderAuquierC.Presenter;

public class JsonRepository : IDataRepository
{
    private readonly List<Book> _books = new List<Book>();
    private readonly string _filePath;
    private readonly string _fileName;

    public JsonRepository(string path, string fileName)
    {
        _filePath = path;
        _fileName = fileName;
    }

    public List<BookDTO> GetData()
    {
        try
        {
            var pathFile = Path.Join(_filePath, _fileName);
            if (!Directory.Exists(_filePath))
            {
                throw new DirectoryNotFoundException($"Le dossier {_filePath} n'a pas été trouvé.");
            }
            if (!File.Exists(pathFile))
            {
                throw new FileNotFoundException($"Le fichier {_filePath} n'a pas été trouvé dans le répertoir {_filePath}.");
            }
            var result = JsonConvert.DeserializeObject<List<BookDTO>>(File.ReadAllText(pathFile));
            return result ?? new List<BookDTO>();
        }
        catch(DirectoryNotFoundException e)
        {
            throw new DirectoryNotFoundException(e.Message);
        }
        catch(FileNotFoundException e)
        {
            throw new FileNotFoundException(e.Message);
        }
    }
}