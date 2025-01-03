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
    }
}
