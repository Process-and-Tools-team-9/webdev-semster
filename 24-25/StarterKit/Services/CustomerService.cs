using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var customers = await _context.Customer.ToListAsync();
        if(customers == null)
        {
            return null;
        }
        return customers;
    }

    public async Task<bool> UpdateCustomerAsync(CustomerBody customerBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM Customer WHERE CustomerId = @Id;";
        var updateQuery = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE CustomerId = @CustomerId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        // Check if the venue exists
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@CustomerId", customerBody.CustomerId);
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;

            if (!exists)
            {
                return false;
            }
        }

        // Update the venue
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@FirstName", customerBody.FirstName);
            updateCommand.Parameters.AddWithValue("@LastName", customerBody.LastName);
            updateCommand.Parameters.AddWithValue("@Email", customerBody.Email);
            updateCommand.Parameters.AddWithValue("@CustomerId", customerBody.CustomerId);

            var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var custToRemove = await _context.Customer.Where(x => x.CustomerId == id).FirstOrDefaultAsync();
        if(custToRemove == null)
        {
            return false;
        }
        _context.Customer.Remove(custToRemove);
        await _context.SaveChangesAsync();
        return true;
    }
}