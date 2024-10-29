namespace BusinessLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface ITitleService
    {
        Task<TitleDTO> GetTitleByIdAsync(string tconst);
        Task<IEnumerable<TitleDTO>> SearchTitlesAsync(string query);
        Task<IEnumerable<TitleDTO>> GetTopRatedTitlesAsync(int count);
    }
}
