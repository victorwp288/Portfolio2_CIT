namespace BusinessLayer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;
    using BusinessLayer.Interfaces;
    using DataAcessLayer.Context;
    using Microsoft.EntityFrameworkCore;

    public class SearchService : ISearchService
    {
        private readonly ImdbContext _context;

        public SearchService(ImdbContext context)
        {
            _context = context;
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
