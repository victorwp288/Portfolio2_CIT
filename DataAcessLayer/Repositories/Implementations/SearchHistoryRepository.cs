using DataAcessLayer.Entities.Users;
using Npgsql;
using System.Threading.Tasks;
using DataAcessLayer.Repositories.Interfaces;
namespace DataAcessLayer.Repositories.Implementations
{
    public class SearchHistoryRepository : BaseRepository, ISearchHistoryRepository
    {
        public SearchHistoryRepository(string connectionString) : base(connectionString) { }

        public async Task LogSearchAsync(int userId, string searchQuery)
        {
            const string sql = "SELECT log_user_search(@userId, @searchQuery)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@searchQuery", searchQuery)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }
    }
}