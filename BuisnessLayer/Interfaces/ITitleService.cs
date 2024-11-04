namespace BusinessLayer.Interfaces
{
    using BuisnessLayer.DTOs;
    using BusinessLayer.DTOs;
    using System.Threading.Tasks;

    public interface ITitleService
    {
        Task<TitleDTO> GetTitleByIdAsync(string tconst);
        Task<PagedResultDTO<TitleDTO>> SearchTitlesAsync(string query, int page, int pageSize);
        Task<PagedResultDTO<TitleDTO>> GetTopRatedTitlesAsync(int page, int pageSize);

    }
}
