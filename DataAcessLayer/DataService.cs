using DataAcessLayer.Movies;
using DataAcessLayer.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataAcessLayer
{
    public class DataService : IDataService
    {
        private readonly ImdbContext _context;

        public DataService(ImdbContext context)
        {
            _context = context;
        }

        public IList<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool LoginUser(string inputUsername, string inputPassword)
        {
            var result = Call<bool>($"SELECT login_user('{inputUsername}', '{inputPassword}')");
            return result;
        }

        public T Call<T>(string sql)
        {
            using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            connection.Open();
            using var command = new NpgsqlCommand(sql, connection);
            return (T)command.ExecuteScalar();
        }


        public IList<TitleBasic> GetTitleBasics(int page, int pagesize)
        {
            return _context.TitleBasics
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToList();
        }

        public int GetNumberOfTitleBasics()
        {
            
            return _context.TitleBasics.Count();
        }
    }

}
