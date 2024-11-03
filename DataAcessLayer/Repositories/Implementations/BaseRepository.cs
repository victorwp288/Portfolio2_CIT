using Npgsql;

namespace DataAcessLayer.Repositories.Implementations

{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
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

        protected async Task<T> ExecuteScalarAsync<T>(string sql, params NpgsqlParameter[] parameters)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return (T)result;
        }
    }
}