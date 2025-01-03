namespace MovieAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public int LoginCount { get; set; } = 1; // Default login count is 1
        public string? Role { get; set; }
    }
}