using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Services;

public class VenueService : IVenueService{
    private readonly DatabaseContext _context;

    public VenueService(DatabaseContext context){
        _context = context;
    }


    public async Task<bool> AddVenueAsync(VenueBody venueBody)
    {
        var query = "INSERT INTO Venue (Name, Capacity) VALUES (@Name, @Capacity);";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Name", venueBody.Name);
        command.Parameters.AddWithValue("@Capacity", venueBody.Capacity);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<Venue> GetVenueAsync(int id)
    {
        var vanue = await _context.Venue.Where(x => x.VenueId == id).FirstOrDefaultAsync();
        if(vanue == null)
        {
            return null;
        }
        return vanue;
    }


    public async Task<List<Venue>> GetAllVenuesAsync()
    {
        var venues = await _context.Venue.ToListAsync();
        if(venues == null)
        {
            return null;
        }
        return venues;
    }

    public async Task<bool> UpdateVenueAsync(VenueBody venueBody)
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
                return false;
            }
        }

        // Update the venue
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@Name", venueBody.Name);
            updateCommand.Parameters.AddWithValue("@Capacity", venueBody.Capacity);
            updateCommand.Parameters.AddWithValue("@VenueId", venueBody.VenueId);

            var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }

    public async Task<bool> DeleteVenueAsync(int id)
    {
        var venToRemove = await _context.Venue.Where(x => x.VenueId == id).FirstOrDefaultAsync();
        if(venToRemove == null)
        {
            return false;
        }
        _context.Venue.Remove(venToRemove);
        await _context.SaveChangesAsync();
        return true;
    }
}