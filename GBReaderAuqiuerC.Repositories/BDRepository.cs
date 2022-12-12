﻿using System.Data;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Repositories.DTO;
using GBReaderAuquierC.Repositories.Exceptions;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SearchOption = GBReaderAuquierC.Infrastructures.SearchOption;

namespace GBReaderAuquierC.Repositories
{
    public class BDRepository : IDataRepository
    {
        private readonly MySqlClientFactory _factory;

        private readonly string _sessionRepo = Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36", "e200106-session.json");
        // private DbProviderFactory _factory;

        private readonly string _connectionString;

        public BDRepository(string providerName, DbInformations info)
        {
            try
            {
                _factory = MySqlClientFactory.Instance;
                // _factory = DbProviderFactories.GetFactory(providerName);
                _connectionString = $"server={info.DbUrl};" +
                                    $"database={info.DbName};" +
                                    $"uid={info.DbUser};" +
                                    $"pwd={info.DbPassword}";
            }
            catch (ArgumentException e)
            {
                throw new UnableToConnectException($"Unable to load prodiver {providerName}",e);
            }
        }

        public IEnumerable<Book> GetBooks(int begin = 0, int end = 0)
        {
            IList<Book> result = new List<Book>();
            try
            {
                using (IDbConnection con = _factory.CreateConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    using var selectCmd = con.CreateCommand();
                    selectCmd.CommandText = "SELECT b.id_book, b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author " +
                                            "FROM book b " +
                                            "LIMIT @begin, @end";
                    var beginParam = selectCmd.CreateParameter();
                    beginParam.ParameterName = "@begin";
                    beginParam.Value = begin;
                    beginParam.DbType = DbType.Int32;
                    selectCmd.Parameters.Add(beginParam);
                    var endParam = selectCmd.CreateParameter();
                    endParam.ParameterName = "@end";
                    endParam.Value = end;
                    endParam.DbType = DbType.Int32;
                    selectCmd.Parameters.Add(endParam);
                    using (IDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new BookDTO(
                                reader["title"] != null ? reader["title"] as string : null,
                                reader["resume"] != null ? reader["resume"] as string : null,
                                reader["author"] != null ? reader["author"] as string : null,
                                reader["isbn"] != null ? reader["isbn"] as string : null,
                                reader["imgPath"] != null ? reader["imgPath"] as string : null
                            );
                            result.Add(Mapper.ConvertToBook(book));
                        }
                    }
                    con.Close();
                }
            }
            catch(UnableToConnectException e)
            {
                throw new DataManipulationException("La connection à la base de donnée n'a pas pu se faire", e);
            }
            catch (MySqlException e) 
            {
                if(e.Number == 1042)
                {
                    throw new DataManipulationException("La connection à la base de donnée n'a pas pu se faire", e);
                }
                throw new DataManipulationException("Une erreur est survenue lorsde la récupération des livres.", e);
            }
            
            return result;
        }

        private IList<PageDTO> GetPages(int idBook)
        {
            IList<PageDTO> result = new List<PageDTO>();
            if (idBook != -1)
            {
                try
                {
                    using (IDbConnection con = _factory.CreateConnection())
                    {
                        con.ConnectionString = _connectionString;
                        con.Open();
                        using var selectCmd = con.CreateCommand();
                        selectCmd.CommandText = "SELECT id_page, content " +
                                                "FROM page " +
                                                "WHERE id_book = @id_book";
                        var param = selectCmd.CreateParameter();
                        param.ParameterName = "@id_book";
                        param.Value = idBook;
                        param.DbType = DbType.Int32;
                        selectCmd.Parameters.Add(param);
                        using (IDataReader reader = selectCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var page = new PageDTO(reader["content"] != null ? reader["content"] as string : null);
                                page.Choices = GetChoices(reader["id_page"] != null ? (int)reader["id_page"] : -1);
                                result.Add(page);
                            }
                        }
                    }
                }
                catch (ArgumentException e)
                {
                    throw new DataManipulationException("The parameters of the query are incorrects.", e);
                }
            }
            return result;
        }

        private Dictionary<string, string> GetChoices(int idPage)
        {
            Dictionary<string, string> result = new();
            try
            {
                using (IDbConnection con = _factory.CreateConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    using var selectCmd = con.CreateCommand();
                    selectCmd.CommandText = "SELECT p.content, c.content as target_content " +
                                            "FROM choice p " +
                                            "JOIN page c ON c.id_page = p.id_target " +
                                            "WHERE p.id_page = @id_page";
                    var param = selectCmd.CreateParameter();
                    param.ParameterName = "@id_page";
                    param.Value = idPage;
                    param.DbType = DbType.Int32;
                    selectCmd.Parameters.Add(param);
                    using (IDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(
                                reader["content"] != null ? reader["content"] as string : null,
                                reader["target_content"] != null ? reader["target_content"] as string : null
                            );
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public Book Search(string isbn)
        {
            var dto = GetDtoFor(isbn);
            return dto != null ? Mapper.ConvertToBook(dto) : null;
        }

        private BookDTO GetDtoFor(string isbn)
        {
            BookDTO dto = null;
            try
            {
                using (IDbConnection con = _factory.CreateConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    using var selectCmd = con.CreateCommand();
                    selectCmd.CommandText = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                            "FROM book b " +
                                            "WHERE b.isbn = @isbn";
                    var param = selectCmd.CreateParameter();
                    param.ParameterName = "@isbn";
                    param.Value = isbn;
                    param.DbType = DbType.String;
                    selectCmd.Parameters.Add(param);
                    using (IDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto = new BookDTO(
                                reader["title"] != null ? reader["title"] as string : null,
                                reader["resume"] != null ? reader["resume"] as string : null,
                                reader["author"] != null ? reader["author"] as string : null,
                                reader["isbn"] != null ? reader["isbn"] as string : null,
                                reader["imgPath"] != null ? reader["imgPath"] as string : null,
                                "1.2"
                            );
                            dto.Id = reader["id_book"] != null ? (int)reader["id_book"] : -1;
                        }
                    }
                }
            }
            catch(UnableToConnectException e)
            {
                throw new DataManipulationException("The connection to the database failed.", e);
            }
            catch (MySqlException e)
            {
                throw new DataManipulationException("", e);
            }
            return dto;
        }

        public Book LoadBook(string isbn)
        {
            var dto = GetDtoFor(isbn);
            if (dto != null)
            {
                dto.Pages = GetPages(dto.Id);
                return Mapper.ConvertToBook(dto);
            }
            return null;
        }

        public void SaveSession(Session session)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(_sessionRepo)))
                {
                    // TODO : modifier messages d'erreurs
                    throw new DirectoryNotFoundException($"Le dossier {Path.GetDirectoryName(_sessionRepo)} n'a pas été trouvé.");
                }

                if (!File.Exists(_sessionRepo))
                {
                    throw new FileNotFoundException(
                        $"Le fichier {Path.GetFileName(_sessionRepo)} n'a pas été trouvé dans le répertoire {Path.GetDirectoryName(_sessionRepo)}.");
                }

                try
                {
                    File.WriteAllText(_sessionRepo, 
                        JsonConvert.SerializeObject(Mapper.ConvertToDTO(session)));
                }
                catch (JsonReaderException)
                {
                }
                catch (IOException)
                {
                }
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
        
        public void LoadSession(Session session)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(_sessionRepo)))
                {
                    // TODO : modifier messages d'erreurs
                    throw new DirectoryNotFoundException($"Le dossier {Path.GetDirectoryName(_sessionRepo)} n'a pas été trouvé.");
                }

                if (!File.Exists(_sessionRepo))
                {
                    throw new FileNotFoundException(
                        $"Le fichier {Path.GetFileName(_sessionRepo)} n'a pas été trouvé dans le répertoire {Path.GetDirectoryName(_sessionRepo)}.");
                }

                try
                {
                    session.History = Mapper.ConvertToSession(JsonConvert.DeserializeObject<SessionDTO>(File.ReadAllText(_sessionRepo))).History;
                }
                catch (JsonReaderException)
                {
                }
                catch (IOException)
                {
                }
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

        public IEnumerable<Book> SearchBooks(string search, SearchOption option, RangeArg arg)
        {
            // TODO : Créer le corps de la méthode
            IList<Book> result = new List<Book>();
            if(arg.Begin < 0 || arg.End < 0) { return result; }
            string query = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                  "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                                  "FROM book b " +
                                  "WHERE b.isbn LIKE @search OR b.title LIKE @search " +
                                  "LIMIT @begin, @end";
            switch (option)
            {
                case SearchOption.FilterIsbn:
                    query = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                            "FROM book b " +
                            "WHERE b.isbn LIKE @search " +
                            "LIMIT @begin, @end";
                    break;
                case SearchOption.FilterTitle:
                    query = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author, b.id_book " +
                            "FROM book b " +
                            "WHERE b.title LIKE @search " +
                            "LIMIT @begin, @end";
                    break;
            }
            try
            {
                using (IDbConnection con = _factory.CreateConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    using var selectCmd = con.CreateCommand();
                    selectCmd.CommandText = query;
                    AddParameter(selectCmd, "@search", $"%{search}%", DbType.String);
                    AddParameter(selectCmd, "@begin", arg.Begin, DbType.Int32);
                    AddParameter(selectCmd, "@end", arg.End, DbType.Int32);
                    using (IDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new BookDTO(
                                reader["title"] != null ? reader["title"] as string : null,
                                reader["resume"] != null ? reader["resume"] as string : null,
                                reader["author"] != null ? reader["author"] as string : null,
                                reader["isbn"] != null ? reader["isbn"] as string : null,
                                reader["imgPath"] != null ? reader["imgPath"] as string : null
                            );
                            result.Add(Mapper.ConvertToBook(book));
                        }
                    }
                    con.Close();
                }
            }
            catch(UnableToConnectException e)
            {
                throw new DataManipulationException("La connection à la base de donnée n'a pas pu se faire", e);
            }
            catch (MySqlException e) 
            {
                if(e.Number == 1042)
                {
                    throw new DataManipulationException("La connection à la base de donnée n'a pas pu se faire", e);
                }
                throw new DataManipulationException("Une erreur est survenue lorsde la récupération des livres.", e);
            }
            
            return result;
        }

        private void AddParameter(IDbCommand command, string name, object value, DbType type)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            param.DbType = type;
            command.Parameters.Add(param);
        }
    }
}