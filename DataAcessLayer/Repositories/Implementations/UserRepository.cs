using DataAcessLayer.Repositories.Interfaces;
using Npgsql;

namespace DataAcessLayer.Repositories.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            const string sql = "SELECT login_user(@username, @password)";
            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password)
            };

            return await ExecuteScalarAsync<bool>(sql, parameters);
        }

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            const string sql = "SELECT register_user(@username, @email, @password)";
            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@password", password)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task UpdateUserRoleAsync(int userId, string newRole)
        {
            const string sql = "SELECT update_user_role(@userId, @newRole::user_role)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@newRole", newRole)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task UpdateUserPasswordAsync(int userId, string newPassword)
        {
            const string sql = "SELECT update_user_password(@userId, @newPassword)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@newPassword", newPassword)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task UpdateUserEmailAsync(int userId, string newEmail)
        {
            const string sql = "SELECT update_user_email(@userId, @newEmail)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@newEmail", newEmail)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }
    }
}