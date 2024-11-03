using DataAcessLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Functions;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IBookmarkRepository
    {
        Task ManageBookmarkAsync(int userId, string movieId, string note);
        Task AddBookmarkAsync(int userId, string movieId, string note);
        Task LogUserSearchAsync(int userId, string searchQuery);
        Task<IEnumerable<UserBookmarkDto>> GetUserBookmarksAsync(int userId);
    }
}
