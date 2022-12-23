using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using Newtonsoft.Json;
using SearchOption = GBReaderAuquierC.Infrastructures.SearchOption;

namespace GBReaderAuquierC.Repositories;

public class JsonRepository : IDataRepository
{
    private readonly List<Book> _books = new();
    private readonly string _filePath;
    private readonly string _fileName;

    public JsonRepository(string path, string fileName)
    {
        _filePath = path;
        _fileName = fileName;
    }

    private void LoadBooks()
    {
        FileAndDirectoryExists();
        try
        {
            _books.Clear();
            JsonConvert.DeserializeObject<List<BookDTO>>(File.ReadAllText(Path.Join(_filePath, _fileName)))?.ForEach(b =>
            {
                if (b != null)
                {
                    _books.Add(Mapper.ConvertToBook(b));
                }
            });
        }
        catch (JsonReaderException e)
        {
            throw new DataManipulationException("Une erreur s'est produite lors de la récupération des données.", e);
        }
        catch (IOException e)
        {
            throw new DataManipulationException("Une erreur s'est produite lors de la récupération des données.", e);
        }
    }

    public IEnumerable<Book> GetBooks(int begin = 0, int end = 0)
    {
        LoadBooks();
        return new List<Book>(_books).GetRange(begin, Math.Min(end, _books.Count));
    }

    private void FileAndDirectoryExists()
    {
        string pathFile = Path.Join(_filePath, _fileName);
        if (!Directory.Exists(_filePath))
        {
            // TODO : modifier messages d'erreurs
            throw new DirectoryNotFoundException($"Le dossier {_filePath} n'a pas été trouvé.");
        }

        if (!File.Exists(pathFile))
        {
            throw new FileNotFoundException(
                $"Le fichier {_filePath} n'a pas été trouvé dans le répertoir {_filePath}.");
        }
    }

    public List<BookDTO> GetData()
    {
        try
        {
            _books.Clear();
            var result = new List<BookDTO>();
            var recup = new List<BookDTO>();
            var pathFile = Path.Join(_filePath, _fileName);
            if (!Directory.Exists(_filePath))
            {
                // TODO : modifier messages d'erreurs
                throw new DirectoryNotFoundException($"Le dossier {_filePath} n'a pas été trouvé.");
            }

            if (!File.Exists(pathFile))
            {
                throw new FileNotFoundException(
                    $"Le fichier {_filePath} n'a pas été trouvé dans le répertoir {_filePath}.");
            }

            try
            {
                recup = JsonConvert.DeserializeObject<List<BookDTO>>(File.ReadAllText(pathFile));
            }
            catch (JsonReaderException)
            {
                return new List<BookDTO>();
            }
            catch (IOException)
            {
                return new List<BookDTO>();
            }
            recup?.ForEach(b =>
            {
                Book get;
                if (b != null && (get = Mapper.ConvertToBook(b)) != null)
                {
                        result.Add(b);
                        _books.Add(get);
                }
            });
            return result.Count > 0 && recup != null ? result : new List<BookDTO>();
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

    public Book Search(string isbn)
    {
        try
        {
            return _books.First(b => b[BookAttribute.Isbn].Contains(isbn));
        }
        catch (Exception)
        {
            throw new NoBooksFindException("No book find for the search: " + isbn);
        }
    }

    public Book LoadBook(string isbn) => Search(isbn);

    public class NoBooksFindException : Exception
    {
        public NoBooksFindException(string message) : base(message) {}
    }

    public IEnumerable<Book> SearchBooks(string search, SearchOption Option, RangeArg arg = null)
    {
        // TODO : Créer le corps de la méthode
        IList<Book> result = new List<Book>();
        return result;
    }
}