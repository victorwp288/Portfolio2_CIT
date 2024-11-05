namespace BusinessLayer.Services
{
    using BusinessLayer.DTOs;
    using BusinessLayer.Interfaces;
    using DataAcessLayer.Context;
    using DataAcessLayer.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SearchService : ISearchService
    {
        private readonly ImdbContext _context;
        private readonly IMovieSearchRepository _movieSearchRepository;
    	private readonly ISearchHistoryRepository _searchHistoryRepository;

        public SearchService(
            ImdbContext context, 
            IMovieSearchRepository movieSearchRepository, 
            ISearchHistoryRepository searchHistoryRepository)
        {
            _context = context;
            _movieSearchRepository = movieSearchRepository;
            _searchHistoryRepository = searchHistoryRepository;
        }

        public async Task<IEnumerable<SearchResultDTO>> SearchTitleByDatabaseAsync(string query)
        {
            var titleResults = await _context.TitleBasics
                                             .Where(t => t.PrimaryTitle.Contains(query))
                                             .Select(t => new SearchResultDTO
                                             {
                                                 Id = t.Tconst,
                                                 Type = "Title",
                                                 Name = t.PrimaryTitle
                                             })
                                             .ToListAsync();

            return titleResults;
        }

		public async Task<IEnumerable<SearchResultDTO>> SearchTitleByDatabaseAsync(string query, int userId)
        {

            await _searchHistoryRepository.LogSearchAsync(userId, query);

            var titleResults = await _context.TitleBasics
                                             .Where(t => t.PrimaryTitle.Contains(query))
                                             .Select(t => new SearchResultDTO
                                             {
                                                 Id = t.Tconst,
                                                 Type = "Title",
                                                 Name = t.PrimaryTitle
                                             })
                                             .ToListAsync();

            return titleResults;
        }

        public async Task<IEnumerable<SearchResultDTO>> SearchTitleAsync(string query)
        {
            var titleResults = await _movieSearchRepository.SearchMoviesAsync(query);
            return titleResults.Select(t => new SearchResultDTO
            {
                Id = t.Tconst,
                Type = "Title",
                Name = t.PrimaryTitle
            });
        }

		        public async Task<IEnumerable<SearchResultDTO>> SearchTitleAsync(string query, int userId)
        {
			await _searchHistoryRepository.LogSearchAsync(userId, query);
            var titleResults = await _movieSearchRepository.SearchMoviesAsync(query);
            return titleResults.Select(t => new SearchResultDTO
            {
                Id = t.Tconst,
                Type = "Title",
                Name = t.PrimaryTitle
            });
        }


        public async Task<IEnumerable<SearchResultDTO>> SearchPersonNameAsync(string query)
        {
            var personResults = await _context.NameBasics
                                              .Where(p => p.PrimaryName.Contains(query))
                                              .Select(p => new SearchResultDTO
                                              {
                                                  Id = p.Nconst,
                                                  Type = "Person",
                                                  Name = p.PrimaryName
                                              })
                                              .ToListAsync();

            return personResults;
        }

        public async Task<IEnumerable<SearchResultDTO>> SearchAsync(string query)
        {
            var titleResults = await _context.TitleBasics
                                             .Where(t => t.PrimaryTitle.Contains(query))
                                             .Select(t => new SearchResultDTO
                                             {
                                                 Id = t.Tconst,
                                                 Type = "Title",
                                                 Name = t.PrimaryTitle
                                             })
                                             .ToListAsync();

            var personResults = await _context.NameBasics
                                              .Where(p => p.PrimaryName.Contains(query))
                                              .Select(p => new SearchResultDTO
                                              {
                                                  Id = p.Nconst,
                                                  Type = "Person",
                                                  Name = p.PrimaryName
                                              })
                                              .ToListAsync();

            var combinedResults = titleResults.Concat(personResults).ToList();
            return combinedResults;
        }
    }
}
