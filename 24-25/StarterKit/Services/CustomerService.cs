using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;

namespace StarterKit.Services;

public class CustomerService : ICustomerService{
    private readonly DatabaseContext _context;

    public CustomerService(DatabaseContext context){
        _context = context;
    }


    public async Task AddCustomerAsync(CustomerBody customerBody)
    {
        var query = "INSERT INTO Customer (Username, Password, Email) VALUES (@Username, @Password, @Email)";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", customerBody.FirstName);
        command.Parameters.AddWithValue("@Password", customerBody.LastName);
        command.Parameters.AddWithValue("@Email", customerBody.Email);

        await command.ExecuteNonQueryAsync();
    }
}