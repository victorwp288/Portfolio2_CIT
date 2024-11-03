using DataAcessLayer.Context;
using Npgsql;

namespace DataAcessLayer.Repositories.Implementations

{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        protected readonly ImdbContext _context;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected BaseRepository(ImdbContext context)
        {
            _context = context;
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<NpgsqlDataReader, T> mapper, params NpgsqlParameter[] parameters)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            var results = new List<T>();
            while (await reader.ReadAsync())
            {
                results.Add(mapper(reader));
            }
            return results;
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, NpgsqlParameter[] parameters)
        {
            if (_context != null)
            {
                throw new NotSupportedException("Direct SQL execution is not supported when using DbContext");
            }

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            var result = await command.ExecuteScalarAsync();
            return (T)result;
        }
    }
}