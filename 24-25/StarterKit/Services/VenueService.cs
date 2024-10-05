using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;

namespace StarterKit.Services;

public class VenueService : IVenueService{
    private readonly DatabaseContext _context;

    public VenueService(DatabaseContext context){
        _context = context;
    }


    public async Task AddVenueAsync(VenueBody venueBody)
    {
        var query = "INSERT INTO Venue (Name, Capacity) VALUES (@Name, @Capacity);";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Name", venueBody.Name);
        command.Parameters.AddWithValue("@Capacity", venueBody.Capacity);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<Venue> GetVenueAsync(int id)
    {
        var query = "SELECT * FROM Venue WHERE VenueId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var venue = new Venue
            {
                VenueId = reader.GetInt32(reader.GetOrdinal("VenueId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                // Map other properties as needed
            };

            return venue;
        }

        return null; // or throw an exception if the venue is not found
    }

    public async Task UpdateVenueAsync(VenueBody venueBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM Venue WHERE VenueId = @VenueId;";
        var updateQuery = "UPDATE Venue SET Name = @Name, Capacity = @Capacity WHERE VenueId = @VenueId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        // Check if the venue exists
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@VenueId", venueBody.VenueId);
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;

            if (!exists)
            {
                throw new Exception("No venue with the specified ID exists.");
            }
        }

        // Update the venue
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@Name", venueBody.Name);
            updateCommand.Parameters.AddWithValue("@Capacity", venueBody.Capacity);
            updateCommand.Parameters.AddWithValue("@VenueId", venueBody.VenueId);

            await updateCommand.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteVenueAsync(int id)
    {
        var query = "DELETE FROM Venue WHERE VenueId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}