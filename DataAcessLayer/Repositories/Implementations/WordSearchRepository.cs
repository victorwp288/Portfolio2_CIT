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
    public class WordSearchRepository : BaseRepository, IWordSearchRepository
    {
        public WordSearchRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<WordAndFrequency>> WordToWordsQueryAsync(int resultLimit, params string[] keywords)
        {
            const string sql = "SELECT * FROM word_to_words_query(@resultLimit, @keywords)";
            var parameters = new[]
            {
                new NpgsqlParameter("@resultLimit", resultLimit),
                new NpgsqlParameter("@keywords", keywords)
            };

            return await QueryAsync(sql, reader => new WordAndFrequency
            {
                Word = reader.GetString(0),
                Frequency = reader.GetInt32(1)
            }, parameters);
        }
    }
}