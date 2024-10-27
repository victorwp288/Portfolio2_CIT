using DataLayer;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace DataService;
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
}


