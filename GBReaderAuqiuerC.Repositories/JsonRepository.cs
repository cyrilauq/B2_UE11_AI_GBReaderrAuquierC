using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;
using Newtonsoft.Json;

namespace GBReaderAuquierC.Presenter;

public class JsonRepository : IDataRepository
{
    public void addBook()
    {
        throw new NotImplementedException();
    }

    public List<BookDTO> getData(string path, string file)
    {
        var pathFile = Path.Join(path, file);
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Le dossier {path} n'a pas été trouvé.");
        }
        if (!File.Exists(pathFile))
        {
            throw new FileNotFoundException($"Le fichier {file} n'a pas été trouvé dans le répertoir {path}.");
        }
        var result = JsonConvert.DeserializeObject<List<BookDTO>>(File.ReadAllText(pathFile));
        return result ?? new List<BookDTO>();
    }

    public void remove()
    {
        throw new NotImplementedException();
    }
}