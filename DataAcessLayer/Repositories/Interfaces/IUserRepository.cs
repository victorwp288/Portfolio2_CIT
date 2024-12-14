using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> LoginUserAsync(string username, string password);
        Task RegisterUserAsync(string username, string email, string password);
        Task UpdateUserRoleAsync(int userId, string newRole);
        Task<bool> UpdateUserPasswordAsync(int userId, string newPassword);
        Task UpdateUserEmailAsync(int userId, string newEmail);
    }
}
