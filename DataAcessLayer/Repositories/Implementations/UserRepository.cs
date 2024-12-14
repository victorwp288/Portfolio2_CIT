using DataAcessLayer.Context;
using DataAcessLayer.Entities.Users;
using DataAcessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.HashClass;
using Npgsql;
namespace DataAcessLayer.Repositories.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public UserRepository(ImdbContext context) : base(context)
        {
        }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            // Retrieve user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                // User not found
                return false;
            }

            // Now we have the hashed password and salt from the database
            var hasher = new Hasher();
            bool isVerified = hasher.VerifyPassword(password, user.PasswordHash, user.Salt);

            const string sql = "SELECT login_user(@username, @password)";
            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password)
            };

            return await ExecuteScalarAsync<bool>(sql, parameters);
        }


        /*public async Task<bool> LoginUserAsync(string username, string password)
        {
            if (_context != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
                return user != null;
            }

            const string sql = "SELECT login_user(@username, @password)";
            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password)
            };

            return await ExecuteScalarAsync<bool>(sql, parameters);
        }*/

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            // Validate password length
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            // Create an instance of the hasher
            var hasher = new Hasher();

            // Generate hashed password and salt
            var (hashedPassword, salt) = hasher.HashPassword(password);

            if (_context != null)
            {
                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = hashedPassword,
                    Salt = salt, // Store the salt in the database
                    CreatedAt = DateTime.UtcNow,
                    Role = UserRole.user
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                const string sql = "SELECT register_user(@username, @email, @hashedPassword, @salt)";
                var parameters = new[]
                {
                    new NpgsqlParameter("@username", username),
                    new NpgsqlParameter("@email", email),
                    new NpgsqlParameter("@hashedPassword", hashedPassword),
                    new NpgsqlParameter("@salt", salt)
};

                await ExecuteScalarAsync<object>(sql, parameters);
            }
        }

        public async Task UpdateUserRoleAsync(int userId, string newRole)
        {
            const string sql = "SELECT update_user_role(@userId, @newRole::user_role)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@newRole", newRole)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, string newPassword)
        {
            

            try
            {
                // Finding the user by their ID
                var user = await _context.Users.FindAsync(userId);
                
                // Creating an instance of the hasher
                var hasher = new Hasher();

                // Generate hashed password and salt
                var (hashedPassword, salt) = hasher.HashPassword(newPassword);

                if (user == null)
                {
                    // Handle user not found case (e.g. log it, throw exception, return false)
                    Console.WriteLine($"User with ID {userId} not found.");
                    return false;
                }

                // Update Password Hash and Salt
                user.PasswordHash = hashedPassword;
                user.Salt = salt;

                // Save Changes 
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user password: {ex.Message}");
                return false;
            }
        }

        public async Task UpdateUserEmailAsync(int userId, string newEmail)
        {
            const string sql = "SELECT update_user_email(@userId, @newEmail)";
            var parameters = new[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@newEmail", newEmail)
            };

            await ExecuteScalarAsync<object>(sql, parameters);
        }
    }
}