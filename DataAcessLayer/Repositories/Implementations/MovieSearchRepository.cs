using DataAcessLayer.Context;
using DataAcessLayer.Entities.Movies;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories.Implementations
{
    public class MovieSearchRepository : IMovieSearchRepository
    {
        private readonly ImdbContext _context;

        public MovieSearchRepository(ImdbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TitleBasic>> SearchMoviesAsync(string query) // Change TitleBasics to TitleBasic
        {
            return await _context.TitleBasics
                .Where(t => t.PrimaryTitle.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }
    }
}