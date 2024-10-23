using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using Microsoft.Data.Sqlite;
using EncryptionHelper = StarterKit.Utils.EncryptionHelper;
using StarterKit.Models;

namespace StarterKit.Controllers;


[Route("api/v1/Login")]
public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginBody loginBody)
    {
        // TODO: Impelement login method
        return StatusCode(501);
    }

    [AdminFilter]
    [HttpPost("Admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginBody loginBody)
    {
        if (IsAdminLoggedIn().GetType() == typeof(OkObjectResult)) 
        {
            return BadRequest("An admin is already logged in.");
        }
        if(IsAdminLoggedInCheck())
        {
            return BadRequest("Admin is already logged in.");
        }
        if (string.IsNullOrEmpty(loginBody.Username) || string.IsNullOrEmpty(loginBody.Password))
        {
            return BadRequest("Username and password must not be empty.");
        }
        LoginStatus status = await _loginService.CheckPassword(loginBody.Username, loginBody.Password);
        if(status == LoginStatus.Success)
        {
            HttpContext.Session.SetString(ADMIN_SESSION_KEY.adminLoggedIn.ToString(), loginBody.Username);
            return Ok("Logged in");
        }
        if(status == LoginStatus.IncorrectUsername)
        {
            return Unauthorized("Incorrect username");
        }
        if(status == LoginStatus.IncorrectPassword)
        {
            return Unauthorized("Incorrect password");
        }
        return BadRequest("An unexpected error occurred.");
    }

    [AdminFilter]
    [HttpPost("Admin/Create")]
    public async Task<IActionResult> CreateAdmin([FromBody] Admin admin)
    {
        if (string.IsNullOrEmpty(admin.UserName) || string.IsNullOrEmpty(admin.Password) || string.IsNullOrEmpty(admin.Email))
        {
            return BadRequest("Username, Email and password must not be empty.");
        }
        if(!await _loginService.CreateAdminDB(admin))
        {
            return BadRequest("Admin already exists.");
        }
        return Ok("Admin created successfully.");

    }

    [HttpGet("check")]
    public IActionResult CheckLogin()
    {
        return Ok("Check");
    }

    // TODO: This method should return a status 200 OK when logged in, else 403, unauthorized
    [AdminFilter]
    [HttpGet("IsAdminLoggedIn")]
    public IActionResult IsAdminLoggedIn()
    {
        string username = HttpContext.Session.GetString(ADMIN_SESSION_KEY.adminLoggedIn.ToString());
        
        if (username != null)
        {
            return Ok(new { IsLoggedIn = true, AdminUserName = username });
        }
        
        return Unauthorized(new { IsLoggedIn = false, Message = "Admin is not logged in." });
    }

    public bool IsAdminLoggedInCheck()
    {
        string username = HttpContext.Session.GetString(ADMIN_SESSION_KEY.adminLoggedIn.ToString());
        return username != null;
    }

    [AdminFilter]
    [HttpPost("AdminLogout")]
    public IActionResult Logout()
    {
        if(!IsAdminLoggedInCheck())
        {
            return BadRequest("Admin is not logged in.");
        }
        HttpContext.Session.Remove(ADMIN_SESSION_KEY.adminLoggedIn.ToString());
        return Ok("Logged out successfully.");
    }
}

public class LoginBody
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
