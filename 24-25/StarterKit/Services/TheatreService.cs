using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using System.Data;

namespace StarterKit.Services;

public class TheatreService : ITheatreService{
    private readonly DatabaseContext _context;

    public TheatreService(DatabaseContext context){
        _context = context;
    }


   public async Task AddTheatreAsync(Theatre TheatreBody)
    {
        var query = "INSERT INTO TheatreShow (Title, Description, Price, VenueId) VALUES (@Title, @Description, @Price, @VenueId);";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", TheatreBody.Title);
        command.Parameters.AddWithValue("@Description", TheatreBody.Description);
        command.Parameters.AddWithValue("@Price", TheatreBody.Price);
        command.Parameters.AddWithValue("@VenueId",TheatreBody.VenueId);

        await command.ExecuteNonQueryAsync();
    }


    public async Task<Theatre> GetTheatreAsync(int id)
    {
        var query = "SELECT * FROM TheatreShow WHERE TheatreShowId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var Theatre = new Theatre
            {
                TheatreShowId = reader.GetInt32(reader.GetOrdinal("TheatreShowId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDouble(reader.GetOrdinal("Price")),
                VenueId = reader.GetInt32(reader.GetOrdinal("VenueId"))
                // Map other properties as needed
            };

            return Theatre;
        }

        return null; // or throw an exception if the Theatre is not found
    }


    public async Task<List<Theatre>> GetAllTheatreAsync()
    {
        var query = "SELECT * FROM TheatreShow;";
        var Theatres = new List<Theatre>();

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var Theatre = new Theatre
            {
                TheatreShowId = reader.GetInt32(reader.GetOrdinal("TheatreShowId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDouble(reader.GetOrdinal("Price")),
                VenueId = reader.GetInt32(reader.GetOrdinal("VenueId"))
                // Map other properties as needed
            };

            Theatres.Add(Theatre);
        }

        return Theatres;
    }

    public async Task UpdateTheatreAsync(Theatre TheatreBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM TheatreShow WHERE TheatreShowId = @TheatreShowId;";
        var updateQuery = "UPDATE TheatreShow SET Title = @Title, Description = @Description, Price = @Price, VenueId = @VenueId WHERE TheatreShowId = @TheatreShowId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        // Check if the Theatre exists
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@TheatreShowId", TheatreBody.TheatreShowId);
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;

            if (!exists)
            {
                throw new Exception("No Theatre with the specified ID exists.");
            }
        }

        // Update the Theatre
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@Title", TheatreBody.Title);
            updateCommand.Parameters.AddWithValue("@Description", TheatreBody.Description);
            updateCommand.Parameters.AddWithValue("@TheatreShowId", TheatreBody.TheatreShowId);
            updateCommand.Parameters.AddWithValue("@Price",TheatreBody.Price);
            updateCommand.Parameters.AddWithValue("@VenueId",TheatreBody.VenueId);

            await updateCommand.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteTheatreAsync(int id)
    {
        var query = "DELETE FROM TheatreShow WHERE TheatreShowId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}