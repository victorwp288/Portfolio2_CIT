using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace DataService;
public class DataService : IDataService
{
    public IList<User> GetUsers()
    {
        var db = new ImdbContext();
        return db.Users.ToList();
    }
}

