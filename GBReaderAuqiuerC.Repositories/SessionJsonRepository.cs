using System.ComponentModel;
using System.Runtime.CompilerServices;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Repositories.DTO;
using Newtonsoft.Json;

namespace GBReaderAuquierC.Repositories
{
    public class SessionJsonRepository : ISessionRepository
    {
        private Book _currentBook;
        private Book _book;
        private Page _page;
        private Dictionary<string, ReadingSession> _history = new ();
        
        private readonly string _path;
        private readonly string _fileName;

        public Book CurrentBook
        {
            get => _currentBook; 
            set
            {
                if (value != null)
                {
                    _currentBook = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public Book ReadingBook 
        { 
            set
            {
                if (value != null)
                {
                    _book = value;
                    NotifyPropertyChanged();
                    if (_book.CountPage > 1)
                    {
                        if (!_history.ContainsKey(_book[BookAttribute.Isbn]))
                        {
                            _history[_book[BookAttribute.Isbn]] = new ReadingSession();
                        }
                        ReadingPage = _history.Count == 0 || _history[_book[BookAttribute.Isbn]].Count == 0 ? _book.First : _book[_history[_book[BookAttribute.Isbn]].Last - 1];
                    }
                }
            }
            get => _book; 
        }

        public Page ReadingPage
        {
            get => _page;
            set
            {
                if (value != null)
                {
                    _page = value;
                    if (_page.IsTerminal)
                    {
                        _history.Remove(_book[BookAttribute.Isbn]);
                    }
                    else
                    {
                        _history[_book[BookAttribute.Isbn]].Add(_book.GetNPageFor(_page));
                    }
                    NotifyPropertyChanged();
                }
            }
        }

    public Dictionary<string, ReadingSession> History
    {
        get => _history;
        private set
        {
            _history = value;
            NotifyPropertyChanged();
        }
    }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SessionJsonRepository(string path, string fileName)
        {
            _path = path;
            _fileName = fileName;
        }

        public void LoadSession()
        {
            CreateIfNotExist();

            try
            {
                History = Mapper.ConvertToSession(JsonConvert.DeserializeObject<SessionDto>(File.ReadAllText(Path.Join(_path, _fileName)))).History;
            }
            catch (JsonSerializationException e)
            {
                throw new DataManipulationException("Erreur lors de l'écriture du fichier.", e);
            }
            catch (IOException e)
            {
                throw new DataManipulationException("Une erreur est survenue lors de la récupération des données", e);
            }
        }

        public void SaveSession()
        {
            CreateIfNotExist();
            try
            {
                File.WriteAllText(Path.Join(_path, _fileName),
                    JsonConvert.SerializeObject(Mapper.ConvertToDto(_history)));
            }
            catch (JsonSerializationException e)
            {
                throw new DataManipulationException("Erreur lors de l'écriture du fichier.", e);
            }
            catch (IOException e)
            {
                throw new DataManipulationException("Erreur lors de l'écriture du fichier.", e);
            }
        }

        private void CreateIfNotExist()
        {
            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
                if (!File.Exists(Path.Join(_path, _fileName)))
                {
                    File.Create(Path.Join(_path, _fileName)).Close();
                }
            }
            catch (Exception e)
            {
                throw new DataManipulationException("Erreur lors de la création de la ressource.", e);
            }
        }

        /*private void VerifyFileAndDirectory()
        {
            if (!Directory.Exists(_path))
            {
                throw new DataManipulationException($"Le dossier {_page} n'a pas été trouvé.");
            }

            if (!File.Exists(Path.Join(_path, _fileName)))
            {
                throw new DataManipulationException(
                    $"Le fichier {_fileName} n'a pas été trouvé dans le répertoire {_path}.");
            }
        }*/

        protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}