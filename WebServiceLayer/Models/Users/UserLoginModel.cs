namespace WebServiceLayer.Models.Users
{
    public class UserLoginModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
