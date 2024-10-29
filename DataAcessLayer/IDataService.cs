using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Users;

namespace DataAcessLayer;

public interface IDataService
{
    IList<User> GetUsers();

    User GetUserById(int id);
}
