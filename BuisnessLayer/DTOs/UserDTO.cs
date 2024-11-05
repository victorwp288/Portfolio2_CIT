namespace BusinessLayer.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; } // Maps to UserId in your User entity
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
