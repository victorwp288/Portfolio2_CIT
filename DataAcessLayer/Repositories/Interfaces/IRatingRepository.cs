using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Functions;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task RateMovieAsync(int userId, string movieId, int rating);
        Task<IEnumerable<UserRating>> GetUserRatingsAsync(int userId);
    }
}