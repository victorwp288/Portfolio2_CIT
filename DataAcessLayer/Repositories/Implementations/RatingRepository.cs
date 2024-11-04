using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Repositories.Interfaces;
using Npgsql;

namespace DataAcessLayer.Repositories.Implementations
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        public RatingRepository(string connectionString) : base(connectionString) { }

        public async Task RateMovieAsync(int userId, string movieId, int rating)
        {
            const string sql = "SELECT rate_movie(@userId, @movieId, @rating)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@movieId", movieId),
                new NpgsqlParameter("@rating", rating)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task<IEnumerable<UserRating>> GetUserRatingsAsync(int userId)
        {
            const string sql = "SELECT * FROM user_ratings_reviews WHERE user_id = @userId";
            var parameters = new[] { new NpgsqlParameter("@userId", userId) };

            return await QueryAsync(sql, reader => new UserRating
            {
                UserId = reader.GetInt32(0),
                Tconst = reader.GetString(1),
                Rating = reader.GetInt32(2),
                Review = reader.IsDBNull(3) ? null : reader.GetString(3),
                ReviewDate = reader.GetDateTime(4)
            }, parameters);
        }
    }
}