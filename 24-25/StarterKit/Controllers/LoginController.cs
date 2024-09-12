using System.Text;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using Microsoft.Data.Sqlite;
using StarterKit.Utils;

namespace StarterKit.Controllers;


[Route("api/v1/Login")]
public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginBody loginBody)
    {
        bool isAuthenticated = CheckIfAdmin(loginBody);
        if(isAuthenticated)
        {
            return Ok("Login successful");
        }
        else
        {
            return Unauthorized("Incorrect password");
        }
    }

    public bool CheckIfAdmin(LoginBody loginBody)
    {
        string query = "SELECT COUNT(1) FROM Admin WHERE Username = @Username AND Password = @Password";

        using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        connection.Open();

        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", loginBody.Username);
        command.Parameters.AddWithValue("@Password", EncryptionHelper.EncryptPassword(loginBody.Password));

        int count = Convert.ToInt32(command.ExecuteScalar());
        bool isAuthenticated = count > 0;
        
        return isAuthenticated;
    }


    [HttpGet("IsAdminLoggedIn")]
    public IActionResult IsAdminLoggedIn()
    {
        // TODO: This method should return a status 200 OK when logged in, else 403, unauthorized
        return Unauthorized("You are not logged in");
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        return Ok("Logged out");
    }

}

public class LoginBody
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
