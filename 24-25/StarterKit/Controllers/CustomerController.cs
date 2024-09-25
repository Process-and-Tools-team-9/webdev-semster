using System.Text;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using Microsoft.Data.Sqlite;
using EncryptionHelper = StarterKit.Utils.EncryptionHelper;
using System.Diagnostics.Contracts;

namespace StarterKit.Controllers;

[Route("api/v1/Customer")]


public class CustomerController : Controller{
    [HttpPost("/Create")]
    public IActionResult Create([FromBody] CustomerBody customerbody){
        
        string query = "INSERT INTO Customer (Username, Password) VALUES (@Username, @Password)";
        using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        connection.Open();
        return Ok();
    }
}

public class CustomerBody{
    public int? CustomerId;
    public string? FirstName;
    public string? LastName;
    public string? Email;
}