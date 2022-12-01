using System.Data;
using System.Data.Common;
using GBReaderAuquierC.Domains;
using MySql.Data.MySqlClient;

namespace GBReaderAuquierC.Repositories
{
    public class BDRepository : IDataRepository
    {
        private MySqlClientFactory _factory;
        // private DbProviderFactory _factory;

        private string _connectionString;
        
        public BDRepository(string providerName, string connection)
        {
            try
            {
                _factory = MySqlClientFactory.Instance;
                // _factory = DbProviderFactories.GetFactory(providerName);
                _connectionString = connection;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"Unable to load prodiver {providerName}",e);
            }
        }

        public IList<Book> GetBooks()
        {
            IList<Book> result = new List<Book>();
            IList<BookDTO> temp = new List<BookDTO>();
            try
            {
                using (IDbConnection con = _factory.CreateConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    using var selectCmd = con.CreateCommand();
                    selectCmd.CommandText = "SELECT b.title, b.isbn, b.datePublication, b.imgPath, b.resume, " +
                                            "(SELECT a.name FROM author a WHERE a.id_author = b.id_author) as author " +
                                            "FROM book b";
                    using (IDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Mapper.ConvertToBook(new BookDTO(
                                reader["title"] != null ? reader["title"] as string : null,
                                reader["resume"] != null ? reader["resume"] as string : null,
                                reader["author"] != null ? reader["author"] as string : null,
                                reader["isbn"] != null ? reader["isbn"] as string : null,
                                reader["imgPath"] != null ? reader["imgPath"] as string : null
                            )));
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                
            }
            return result;
        }

        public Book Search(string isbn)
        {
            Book result = null;
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
                            result = Mapper.ConvertToBook(new BookDTO(
                                reader["title"] != null ? reader["title"] as string : null,
                                reader["resume"] != null ? reader["resume"] as string : null,
                                reader["author"] != null ? reader["author"] as string : null,
                                reader["isbn"] != null ? reader["isbn"] as string : null,
                                reader["imgPath"] != null ? reader["imgPath"] as string : null
                            ));
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                
            }
            return result;
        }

        private IList<Page> GetPagesFof(int id_book)
        {
            return new List<Page>();
        }
    }
}