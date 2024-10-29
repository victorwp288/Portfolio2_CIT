using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
using DataAcessLayer.Movies;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class TitleService : ITitleService
    {
        private readonly ImdbContext _context;

        public TitleService(ImdbContext context)
        {
            _context = context;
        }

        public async Task<TitleDTO> GetTitleByIdAsync(string tconst)
        {
            var title = await _context.TitleBasics
                                      .Include(t => t.TitleRating) // Singular
                                      .Include(t => t.MovieGenres) // Plural
                                      .SingleOrDefaultAsync(t => t.Tconst == tconst);

            if (title == null)
                throw new KeyNotFoundException("Title not found.");

            var titleDto = MapTitleToDTO(title);
            return titleDto;
        }

        public async Task<IEnumerable<TitleDTO>> SearchTitlesAsync(string query)
        {
            var titles = await _context.TitleBasics
                                       .Include(t => t.TitleRating)
                                       .Include(t => t.MovieGenres)
                                       .Where(t => t.PrimaryTitle.Contains(query) || t.OriginalTitle.Contains(query))
                                       .ToListAsync();

            var titleDtos = titles.Select(MapTitleToDTO).ToList();
            return titleDtos;
        }

        public async Task<IEnumerable<TitleDTO>> GetTopRatedTitlesAsync(int count)
        {
            var titles = await _context.TitleBasics
                                       .Include(t => t.TitleRating)
                                       .Include(t => t.MovieGenres)
                                       .Where(t => t.TitleRating != null)
                                       .OrderByDescending(t => t.TitleRating.AverageRating)
                                       .Take(count)
                                       .ToListAsync();

            var titleDtos = titles.Select(MapTitleToDTO).ToList();
            return titleDtos;
        }

        // Helper method to map TitleBasic to TitleDTO
        private TitleDTO MapTitleToDTO(TitleBasic title)
        {
            return new TitleDTO
            {
                TConst = title.Tconst,
                PrimaryTitle = title.PrimaryTitle,
                OriginalTitle = title.OriginalTitle,
                IsAdult = title.IsAdult,
                StartYear = title.StartYear,
                EndYear = title.EndYear,
                RunTimeMinutes = title.RunTimeMinutes,
                Genres = title.MovieGenres?.Select(g => g.Genre).ToList(),
                AverageRating = title.TitleRating?.AverageRating,
                NumVotes = title.TitleRating?.NumVotes
            };
        }
    }
}
