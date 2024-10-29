namespace BusinessLayer.Services
{
    using System;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;
    using BusinessLayer.Interfaces;
    using DataAcessLayer;
    using DataAcessLayer.Users;
    using Microsoft.EntityFrameworkCore;



    // Adjusted namespace for User

    public class UserService : IUserService
    {
        private readonly ImdbContext _context;

        public UserService(ImdbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> RegisterUserAsync(UserRegistrationDTO registrationDto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(registrationDto.Email) || string.IsNullOrWhiteSpace(registrationDto.Password))
                throw new ArgumentException("Email and password are required.");

            // Check if email already exists
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == registrationDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email is already registered.");

            // Hash the password (implement a proper hashing mechanism)
            var passwordHash = HashPassword(registrationDto.Password);

            // Create a new User entity
            var user = new User
            {
                Email = registrationDto.Email,
                Username = registrationDto.Username,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            };

            // Add user to the database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Map to UserDTO
            var userDto = new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            };

            return userDto;
        }

        public async Task<UserDTO> AuthenticateUserAsync(string email, string password)
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
        }

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

        public async Task UpdateUserAsync(UserUpdateDTO updateDto)
        {
            var user = await _context.Users.FindAsync(updateDto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Update fields
            user.Email = updateDto.Email ?? user.Email;
            user.Username = updateDto.Username ?? user.Username;
            // Update other fields as necessary

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Helper methods for password hashing
        private string HashPassword(string password)
        {
            // Implement a secure password hashing mechanism (e.g., BCrypt)
            return password; // Placeholder
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Implement password verification
            return password == passwordHash; // Placeholder
        }
    }
}
