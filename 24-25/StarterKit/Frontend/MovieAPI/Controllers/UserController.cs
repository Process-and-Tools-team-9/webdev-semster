// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Data;
using MovieAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MovieContext _context;

        public UsersController(MovieContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            // Basic validation
            if (string.IsNullOrEmpty(loginRequest.Username) ||
                string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            // Look for matching user in DB
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username 
                                       && u.Password == loginRequest.Password);

            if (user == null)
            {
                // Invalid credentials
                return Unauthorized("Invalid username or password.");
            }

            // Valid user found; increment login count
            user.LoginCount++;
            await _context.SaveChangesAsync();

            // Return a safe subset of user info (exclude password)
            return Ok(new 
            { 
                message = "Login successful.", 
                user = new 
                {
                    user.Id,
                    user.Username,
                    user.LoginCount,
                    user.Role
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User registerRequest)
        {
            // 1) Basic validation
            if (string.IsNullOrEmpty(registerRequest.Username) ||
                string.IsNullOrEmpty(registerRequest.Password) ||
                string.IsNullOrEmpty(registerRequest.Email))
            {
                return BadRequest("Username, password and email are required.");
            }

            // 2) Check if username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerRequest.Username);

            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            // 3) Create and save new user
            // For real apps: hash the password before saving to DB!
            var newUser = new User
            {
                Username = registerRequest.Username,
                Password = registerRequest.Password, // Plain-text here; not safe for production
                Email = registerRequest.Email,
                Role = "user",       // default role
                LoginCount = 0
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // 4) Return minimal user info (excluding raw password)
            return Ok(new
            {
                message = "User created successfully.",
                user = new
                {
                    newUser.Email,
                    newUser.Username
                }
            });
        }
    }
}
