using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Implementations
{
    public class PersonSearchRepository : BaseRepository, IPersonSearchRepository
    {
        public PersonSearchRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<SearchName>> SearchNamesAsync(string searchText)
        {
            const string sql = "SELECT * FROM search_names(@searchText)";
            var parameters = new[] { new NpgsqlParameter("@searchText", searchText) };

            return await QueryAsync(sql, reader => new SearchName
            {
                Tconst = reader.GetString(0),
                PrimaryTitle = reader.GetString(1),
                Nconst = reader.GetString(2),
                PrimaryName = reader.GetString(3)
            }, parameters);
        }

        public async Task<IEnumerable<SearchCoPlayer>> SearchCoPlayersAsync(string searchName)
        {
            const string sql = "SELECT * FROM search_co_players(@searchName)";
            var parameters = new[] { new NpgsqlParameter("@searchName", searchName) };

            return await QueryAsync(sql, reader => new SearchCoPlayer
            {
                Nconst = reader.GetString(0),
                PrimaryName = reader.GetString(1),
                Frequency = reader.GetInt32(2)
            }, parameters);
        }

        public async Task<IEnumerable<GetMovieActorsByPopularity>> GetMovieActorsByPopularityAsync(string tconst)
        {
            const string sql = "SELECT * FROM get_movie_actors_by_popularity(@tconst)";
            var parameters = new[] { new NpgsqlParameter("@tconst", tconst) };

            return await QueryAsync(sql, reader => new GetMovieActorsByPopularity
            {
                Nconst = reader.GetString(0),
                PrimaryName = reader.GetString(1),
                WeightedRating = reader.GetDouble(2)
            }, parameters);
        }

        public async Task<IEnumerable<WordAndFrequency>> GetPersonWordsAsync(string name, int limit = 10)
        {
            const string sql = "SELECT * FROM person_words(@name, @limit)";
            var parameters = new[]
            {
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@limit", limit)
            };

            return await QueryAsync(sql, reader => new WordAndFrequency
            {
                Word = reader.GetString(0),
                Frequency = reader.GetInt32(1)
            }, parameters);
        }
    }
}


