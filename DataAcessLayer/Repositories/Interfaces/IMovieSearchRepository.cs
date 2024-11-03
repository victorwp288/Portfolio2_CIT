using DataAcessLayer.Entities.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IMovieSearchRepository
    {
        Task<IEnumerable<TconstAndPrimaryTitle>> SearchMoviesAsync(string searchText);
        Task<IEnumerable<TconstAndPrimaryTitle>> StructuredSearchAsync(string title, string plot, string actor);
        Task<IEnumerable<TconstAndPrimaryTitle>> OtherMoviesLikeThisAsync(string searchName);
        Task<IEnumerable<BestMatchQuery>> BestMatchQueryAsync(string w1, string w2, string w3);
        Task<IEnumerable<TconstAndPrimaryTitle>> ExactMatchQueryAsync(string w1, string w2, string w3);
    }
}
