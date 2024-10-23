using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using System.Data;
namespace StarterKit.Services;
public class TheatreShowDateService : ITheatreShowDateService
{
    private readonly DatabaseContext _context;
    public TheatreShowDateService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task AddTheatreShowDateAsync(_theatreShowDate TheatreShowDateBody)
    {
        var query = "INSERT INTO TheatreShowDate (DateAndTime, TheatreShowId) VALUES (@DateAndTime, @TheatreShowId);";
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();
        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@DateAndTime", TheatreShowDateBody.DateAndTime);
        command.Parameters.AddWithValue("@TheatreShowId", TheatreShowDateBody.TheatreShowId);
        await command.ExecuteNonQueryAsync();
    }
    public async Task<_theatreShowDate> GetTheatreShowDateAsync(int id)
    {
        var query = "SELECT * FROM TheatreShowDate WHERE TheatreShowDateId = @Id;";
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();
        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var _theatreShowDate = new _theatreShowDate
            {
                TheatreShowDateId = reader.GetInt32(reader.GetOrdinal("TheatreShowDateId")),
                DateAndTime = reader.GetString(reader.GetOrdinal("DateAndTime")),
                TheatreShowId = reader.GetInt32(reader.GetOrdinal("TheatreShowId"))
            };
            return _theatreShowDate;
        }
        return null;
    }
    public async Task<List<_theatreShowDate>> GetAllTheatreShowDatesAsync()
    {
        var query = "SELECT * FROM TheatreShowDate;";
        var TheatreShowDates = new List<_theatreShowDate>();
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();
        await using var command = new SqliteCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var _theatreShowDate = new _theatreShowDate
            {
                TheatreShowDateId = reader.GetInt32(reader.GetOrdinal("TheatreShowDateId")),
                DateAndTime = reader.GetString(reader.GetOrdinal("DateAndTime")),
                TheatreShowId = reader.GetInt32(reader.GetOrdinal("TheatreShowId"))
            };
            TheatreShowDates.Add(_theatreShowDate);
        }
        return TheatreShowDates;
    }
    public async Task UpdateTheatreShowDateAsync(_theatreShowDate TheatreShowDateBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM TheatreShowDate WHERE TheatreShowDateId = @TheatreShowDateId;";
        var updateQuery = "UPDATE TheatreShowDate SET DateAndTime = @DateAndTime, TheatreShowId = @TheatreShowId WHERE TheatreShowDateId = @TheatreShowDateId;";
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@TheatreShowDateId", TheatreShowDateBody.TheatreShowDateId);
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;
            if (!exists)
            {
                throw new Exception("No Theatre Show Date with the specified ID exists.");
            }
        }
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@DateAndTime", TheatreShowDateBody.DateAndTime);
            updateCommand.Parameters.AddWithValue("@TheatreShowId", TheatreShowDateBody.TheatreShowId);
            updateCommand.Parameters.AddWithValue("@TheatreShowDateId", TheatreShowDateBody.TheatreShowDateId);
            await updateCommand.ExecuteNonQueryAsync();
        }
    }
    public async Task DeleteTheatreShowDateAsync(int id)
    {
        var query = "DELETE FROM TheatreShowDate WHERE TheatreShowDateId = @Id;";
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();
        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        await command.ExecuteNonQueryAsync();
    }
}