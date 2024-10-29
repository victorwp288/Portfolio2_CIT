namespace BusinessLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDTO>> SearchAsync(string query);
    }
}
