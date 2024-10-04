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


    public async Task AddCustomerAsync(CustomerBody customerBody){
    var query = "INSERT INTO Customer (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
    
    await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
    await connection.OpenAsync();

    await using var command = new SqliteCommand(query, connection);
    command.Parameters.AddWithValue("@FirstName", customerBody.FirstName);
    command.Parameters.AddWithValue("@LastName", customerBody.LastName);
    command.Parameters.AddWithValue("@Email", customerBody.Email);

    try
    {
        await command.ExecuteNonQueryAsync();
    }
    catch (SqliteException ex)
    {
        Console.WriteLine("Error inserting customer: " + ex.Message);
        throw;
    }
}
public async Task<Customer> GetCustomerAsync(int id)
    {
        var query = "SELECT * FROM Customer WHERE CustomerId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var customer = new Customer
            {
                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
                // Map other properties as needed
            };

            return customer;
        }

        return null; // or throw an exception if the venue is not found
    }
}