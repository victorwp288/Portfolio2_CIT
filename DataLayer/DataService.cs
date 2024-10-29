using DataAcessLayer.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace DataAcessLayer;
public class DataService : IDataService
{
    public IList<User> GetUsers()
    {
        var db = new ImdbContext();
        return db.Users.ToList();
    }

    public User GetUserById(int id)
    {
        var db = new ImdbContext();
        return db.Users.Find(id);
    }

    public bool LoginUser(string InputUsername, string InputPassword)
    {
        var db = new ImdbContext();
        var result = Call<bool>(db, $"SELECT login_user('{InputUsername}', '{InputPassword}')");
        return result;
    }



    public T Call<T>(DbContext context, string sql)
    {
        var connection = (NpgsqlConnection)context.Database.GetDbConnection();
        connection.Open();
        var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = sql;
        return (T)command.ExecuteScalar(); //command.ExecuteScalar();
    }
}


