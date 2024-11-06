namespace MovieAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int LoginCount { get; set; } = 1; // Default login count is 1
    }
}