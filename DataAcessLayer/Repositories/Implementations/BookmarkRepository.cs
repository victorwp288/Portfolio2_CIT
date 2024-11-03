using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Entities.Users;
using DataAcessLayer.Repositories.Interfaces;
using Npgsql;

namespace DataAcessLayer.Repositories.Implementations
{
    public class BookmarkRepository : BaseRepository, IBookmarkRepository
    {
        public BookmarkRepository(string connectionString) : base(connectionString) { }

        public async Task ManageBookmarkAsync(int userId, string movieId, string note)
        {
            const string sql = "SELECT manage_bookmark(@userId, @movieId, @note)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@movieId", movieId),
                new NpgsqlParameter("@note", note)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task AddBookmarkAsync(int userId, string movieId, string note)
        {
            const string sql = "SELECT add_bookmark(@userId, @movieId, @note)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@movieId", movieId),
                new NpgsqlParameter("@note", note)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task LogUserSearchAsync(int userId, string searchQuery)
        {
            const string sql = "SELECT log_user_search(@userId, @searchQuery)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@searchQuery", searchQuery)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task<IEnumerable<UserBookmarkDto>> GetUserBookmarksAsync(int userId)
        {
            const string sql = "SELECT * FROM user_bookmarks WHERE user_id = @userId";
            var parameters = new[] { new NpgsqlParameter("@userId", userId) };

            return await QueryAsync(sql, reader => new UserBookmarkDto
            {
                UserId = reader.GetInt32(0),
                Tconst = reader.GetString(1),
                Note = reader.IsDBNull(2) ? null : reader.GetString(2),
                BookmarkDate = reader.GetDateTime(3)
            }, parameters);
        }
    }
}