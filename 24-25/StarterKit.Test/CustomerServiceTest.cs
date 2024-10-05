// using Xunit;
// using Moq;
// using Microsoft.Data.Sqlite;
// using System.Threading.Tasks;
// using StarterKit.Controllers;
// using StarterKit.Services;
// using StarterKit.Models;

// public class CustomerServiceTests
// {
//     [Fact]
//     public async Task AddCustomerAsync_InsertsCustomerIntoDatabase()
//     {
//         // Arrange
//         var connection = new SqliteConnection("DataSource=:memory:");
//         await connection.OpenAsync();
        
//         var command = connection.CreateCommand();
//         command.CommandText = "CREATE TABLE Customer (CustomerId INTEGER PRIMARY KEY, FirstName TEXT, LastName TEXT, Email TEXT)";
//         await command.ExecuteNonQueryAsync();

//         var customerService = new CustomerService(null); 

//         var customerBody = new CustomerBody
//         {
//             FirstName = "John",
//             LastName = "Doe",
//             Email = "john_doe@example.com"
//         };

//         // Act
//         await customerService.AddCustomerAsync(customerBody);

//         // Assert: Verify that the customer was added by checking the table
//         var selectCommand = connection.CreateCommand();
//         selectCommand.CommandText = "SELECT * FROM Customer WHERE Email = @Email";
//         selectCommand.Parameters.AddWithValue("@Email", customerBody.Email);

//         var reader = await selectCommand.ExecuteReaderAsync();
//         Assert.True(reader.HasRows);
//     }
// }
