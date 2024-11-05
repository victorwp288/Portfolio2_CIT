namespace BusinessLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDTO>> SearchAsync(string query);
        Task<IEnumerable<SearchResultDTO>> SearchTitleAsync(string query);
        Task<IEnumerable<SearchResultDTO>> SearchTitleByDatabaseAsync(string query);
        Task<IEnumerable<SearchResultDTO>> SearchPersonNameAsync(string query);
        Task<IEnumerable<SearchResultDTO>> SearchTitleAsync(string query, int userId);
        Task<IEnumerable<SearchResultDTO>> SearchTitleByDatabaseAsync(string query, int userId);
    }
}
