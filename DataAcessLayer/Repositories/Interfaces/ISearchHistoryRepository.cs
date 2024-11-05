namespace DataAcessLayer.Repositories.Interfaces
{
    public interface ISearchHistoryRepository
    {
        Task LogSearchAsync(int userId, string searchQuery);
    }
}