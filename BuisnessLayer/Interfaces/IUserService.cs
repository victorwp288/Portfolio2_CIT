namespace BusinessLayer.Interfaces
{
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface IUserService
    {
        Task<UserDTO> RegisterUserAsync(UserRegistrationDTO registrationDto);
        Task<UserDTO> AuthenticateUserAsync(string email, string password);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(UserUpdateDTO updateDto);
        Task DeleteUserAsync(int userId);
    }
}
