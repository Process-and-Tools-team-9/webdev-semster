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
            if (string.IsNullOrEmpty(loginRequest.Firstname) || string.IsNullOrEmpty(loginRequest.Lastname))
            {
                return BadRequest("Firstname and Lastname are required.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Firstname == loginRequest.Firstname && u.Lastname == loginRequest.Lastname);

            if (user != null)
            {
                // User exists; increment login count
                user.LoginCount += 1;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Login count incremented", user });
            }
            else
            {
                // User does not exist; create a new user
                var newUser = new User
                {
                    Firstname = loginRequest.Firstname,
                    Lastname = loginRequest.Lastname,
                    LoginCount = 1
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(new { message = "New user created", user = newUser });
            }
        }
    }
}
