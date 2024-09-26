using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using Microsoft.Data.Sqlite;
using EncryptionHelper = StarterKit.Utils.EncryptionHelper;

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
        // TODO: Impelement login method
        return StatusCode(501);
    }

    [HttpPost("/login/admin")]
    public IActionResult LoginAdmin([FromBody] LoginBody loginBody)
    {
        string query = "SELECT COUNT(1) FROM Admin WHERE UserName = @Username AND Password = @Password";
        bool isAuthenticated = false;
        using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        connection.Open();

        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", loginBody.Username);
        command.Parameters.AddWithValue("@Password", EncryptionHelper.EncryptPassword(loginBody.Password));
        int count = Convert.ToInt32(command.ExecuteScalar());
        isAuthenticated = count > 0;
        if(isAuthenticated)
        {
            return Ok("Logged in as admin");
        }
        return Unauthorized("Incorrect password");
    }

    [HttpPost("/login/admin/create")]
    public IActionResult CreateAdmin([FromBody] LoginBody loginBody)
    {
        string query = "INSERT INTO Admin (Username, Password) VALUES (@Username, @Password)";
        using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        connection.Open();

        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", loginBody.Username);
        command.Parameters.AddWithValue("@AdminId", "999");

        command.Parameters.AddWithValue("@Password", EncryptionHelper.EncryptPassword(loginBody.Password));
        command.ExecuteNonQuery();
        return Ok("Admin created");
    }

    [HttpGet("/login/check")]
    public IActionResult CheckLogin()
    {
        return Ok("Check");
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
