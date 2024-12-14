namespace BusinessLayer.Services
{
    using BusinessLayer.DTOs;
    using BusinessLayer.Interfaces;
    using DataAcessLayer.Context;
    using DataAcessLayer.Repositories.Implementations;
    using Microsoft.EntityFrameworkCore;
    using Npgsql;
    using System;
    using System.Threading.Tasks;
    using DataAcessLayer.HashClass;
    using BuisnessLayer.Interfaces;


    // Adjusted namespace for User

    public class UserService : IUserService
    {
        private readonly ImdbContext _context;
        private readonly UserRepository _userRepository;
        IBookmarkService _bookmarkService;
        IRatingService _ratingService;
        IHasherService _hasherService;


        public UserService(ImdbContext context, IBookmarkService bookmarkService, IRatingService ratingService,IHasherService hasherService)
        {
            _context = context;
            _userRepository = new UserRepository(context);
            _bookmarkService = bookmarkService;
            _ratingService = ratingService;
            _hasherService = hasherService;
        }

        public async Task<UserDTO> RegisterUserAsync(UserRegistrationDTO registrationDto)
        {
            try
            {              
                
                await _userRepository.RegisterUserAsync(
                    registrationDto.Username,
                    registrationDto.Email,
                    registrationDto.Password
                );

                // Get the newly created user from the database
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == registrationDto.Username);

                return new UserDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user", ex);
            }
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
            Console.WriteLine(isVerified);
            return isVerified;
        }

        /*public async Task<UserDTO> AuthenticateUserAsync(string email, string password)
        {
            // Retrieve the user by email
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new InvalidOperationException("Invalid email or password.");

            // Verify the password
            if (!VerifyPassword(password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password.");

            // Map to UserDTO
            var userDto = new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            };

            return userDto;
        }*/

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            };

            return userDto;
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDTO updateDto)
        {
            var result = await _userRepository.UpdateUserPasswordAsync(
                    updateDto.UserId,
                    updateDto.Password
                );

            if(result)
                return true;
            else return false;

        }

        public async Task DeleteUserAsync(int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _bookmarkService.DeleteAllBookmarksForUserAsync(userId);
                await _ratingService.DeleteAllRatingsForUserAsync(userId);

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (KeyNotFoundException ex)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw the exception to be handled by the caller.
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"An error occurred while deleting user {userId}: {ex.Message}");
                throw;
            }
        }

        // Helper methods for password hashing
        private async Task<string> HashPassword(string password)
        {
            using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            connection.Open();
            using var command = new NpgsqlCommand("SELECT crypt(@password, gen_salt('bf'))", connection);
            command.Parameters.AddWithValue("password", password);
            return (string)command.ExecuteScalar();
        }

        // Helper method for password verification
        private bool VerifyPassword(string password, string passwordHash)
        {
            using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            connection.Open();
            using var command = new NpgsqlCommand("SELECT crypt(@password, @hash) = @hash", connection);
            command.Parameters.AddWithValue("password", password);
            command.Parameters.AddWithValue("hash", passwordHash);
            return (bool)command.ExecuteScalar();
        }
    }
}
