using DataAcessLayer.Entities.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IPersonSearchRepository
    {
        Task<IEnumerable<SearchName>> SearchNamesAsync(string searchText);
        Task<IEnumerable<SearchCoPlayer>> SearchCoPlayersAsync(string searchName);
        Task<IEnumerable<GetMovieActorsByPopularity>> GetMovieActorsByPopularityAsync(string tconst);
        Task<IEnumerable<WordAndFrequency>> GetPersonWordsAsync(string name, int limit = 10);
    }
}
