namespace BusinessLayer.Interfaces
{
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface IRatingService
    {
        Task SubmitUserRatingAsync(UserRatingDTO ratingDto);
        Task<UserRatingDTO> GetUserRatingAsync(int userId, string tconst);
    }
}
