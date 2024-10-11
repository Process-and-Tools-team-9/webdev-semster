using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;

namespace StarterKit.Services;

public class ReservationService : IReservationService
{
    private readonly DatabaseContext _context;

    public ReservationService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddReservationAsync(ReservationBody reservationBody)
    {
        var query = "INSERT INTO Reservation (CustomerId, TheatreShowDateId, AmountOfTickets, Used) VALUES (@CustomerId, @TheatreShowDateId, @AmountOfTickets, @Used)";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerId", reservationBody.CustomerId);
        command.Parameters.AddWithValue("@TheatreShowDateId", reservationBody.TheatreShowDateId);
        command.Parameters.AddWithValue("@AmountOfTickets", reservationBody.AmountOfTickets);
        command.Parameters.AddWithValue("@Used", reservationBody.Used);

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("Error inserting reservation: " + ex.Message);
            throw;
        }
    }

    public async Task<ReservationBody> GetReservationAsync(int id)
    {
        var query = "SELECT * FROM Reservation WHERE ReservationId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var reservation = new ReservationBody
            {
                ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                TheatreShowDateId = reader.GetInt32(reader.GetOrdinal("TheatreShowDateId")),
                AmountOfTickets = reader.GetInt32(reader.GetOrdinal("AmountOfTickets")),
                Used = reader.GetInt32(reader.GetOrdinal("Used"))
                
            };

            return reservation;
        }

        return null; 
    }

    public async Task DeleteReservationAsync(int id)
    {
        var query = "DELETE FROM Reservation WHERE ReservationId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        try
        {
            var result = await command.ExecuteNonQueryAsync();
            if (result == 0)
            {
                throw new Exception("No reservation with the specified ID exists.");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("Error deleting reservation: " + ex.Message);
            throw;
        }
    }

    Task<Reservation> IReservationService.GetReservationAsync(int id)
    {
        throw new NotImplementedException();
    }
}