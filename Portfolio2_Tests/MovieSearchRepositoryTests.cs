using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Repositories.Interfaces;
using Npgsql;

namespace DataAcessLayer.Repositories.Implementations
{
    public class MovieSearchRepository : BaseRepository, IMovieSearchRepository
    {
        public MovieSearchRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<TconstAndPrimaryTitle>> SearchMoviesAsync(string searchText)
        {
            const string sql = "SELECT * FROM search_movies(@searchText)";
            var parameters = new[] { new NpgsqlParameter("@searchText", searchText) };

            return await QueryAsync(sql, reader => new TconstAndPrimaryTitle
            {
                Tconst = reader.GetString(0),
                PrimaryTitle = reader.GetString(1)
            }, parameters);
        }

        public async Task<IEnumerable<BestMatchQuery>> BestMatchQueryAsync(string w1, string w2, string w3)
        {
            const string sql = "SELECT * FROM best_match_query(@w1, @w2, @w3)";
            var parameters = new[]
            {
                new NpgsqlParameter("@w1", w1),
                new NpgsqlParameter("@w2", w2),
                new NpgsqlParameter("@w3", w3)
            };

            return await QueryAsync(sql, reader => new BestMatchQuery
            {
                Tconst = reader.GetString(0),
                Rank = reader.GetInt32(1),
                PrimaryTitle = reader.GetString(2)
            }, parameters);
        }

        public async Task<IEnumerable<TconstAndPrimaryTitle>> StructuredSearchAsync(string title, string plot, string actor)
        {
            const string sql = "SELECT * FROM structured_search(@title, @plot, @actor)";
            var parameters = new[]
            {
                new NpgsqlParameter("@title", title),
                new NpgsqlParameter("@plot", plot),
                new NpgsqlParameter("@actor", actor)
            };

            return await QueryAsync(sql, reader => new TconstAndPrimaryTitle
            {
                Tconst = reader.GetString(0),
                PrimaryTitle = reader.GetString(1)
            }, parameters);
        }

        public async Task<IEnumerable<TconstAndPrimaryTitle>> OtherMoviesLikeThisAsync(string searchName)
        {
            const string sql = "SELECT * FROM other_movies_like_this(@searchName)";
            var parameters = new[] { new NpgsqlParameter("@searchName", searchName) };

            return await QueryAsync(sql, reader => new TconstAndPrimaryTitle
            {
                Tconst = reader.GetString(0),
                PrimaryTitle = reader.GetString(1)
            }, parameters);
        }

        public async Task<IEnumerable<TconstAndPrimaryTitle>> ExactMatchQueryAsync(string w1, string w2, string w3)
        {
            const string sql = "SELECT * FROM exact_match_query(@w1, @w2, @w3)";
            var parameters = new[]
            {
                new NpgsqlParameter("@w1", w1),
                new NpgsqlParameter("@w2", w2),
                new NpgsqlParameter("@w3", w3)
            };

            return await QueryAsync(sql, reader => new TconstAndPrimaryTitle
            {
                Tconst = reader.GetString(0),
                PrimaryTitle = reader.GetString(1)
            }, parameters);
        }

        // Implement other methods similarly
    }
} 