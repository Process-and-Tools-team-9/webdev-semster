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
        var query = "INSERT INTO Reservation (ReservationId, AmountOfTickets, Used, CustomerId, TheatreShowDateId) VALUES (@ReservationId, @AmountOfTickets, @Used, @CustomerId, @TheatreShowDateId);";
        
        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@ReservationId", reservationBody.ReservationId);
        command.Parameters.AddWithValue("@AmountOfTickets", reservationBody.AmountOfTickets);
        command.Parameters.AddWithValue("@Used", reservationBody.Used);
        command.Parameters.AddWithValue("@CustomerId", reservationBody.CustomerId);
        command.Parameters.AddWithValue("@TheatreShowDateId", reservationBody.TheatreShowDateId);

        await command.ExecuteNonQueryAsync();
    }


    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        var query = "SELECT * FROM Reservation;";
        var reservations = new List<Reservation>();

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var reservation = new Reservation
            {
                ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                AmountOfTickets = reader.GetInt32(reader.GetOrdinal("AmountOfTickets")),
                Used = reader.GetBoolean(reader.GetOrdinal("Used")),
                // Map other properties as needed
            };

            reservations.Add(reservation);
        }

        return reservations;
    }


    public async Task<Reservation> GetReservationAsync(int id)
    {
        var query = "SELECT * FROM Reservation WHERE ReservationId = @ReservationId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var reservation = new Reservation
            {
                ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                AmountOfTickets = reader.GetInt32(reader.GetOrdinal("AmountOfTickets")),
                Used = reader.GetBoolean(reader.GetOrdinal("Used")),
                // Map other properties as needed
            };

            return reservation;
        }

        throw new Exception("No reservation with the specified ID exists.");
    }


    public async Task UpdateReservationAsync(ReservationBody reservationBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM Reservation WHERE ReservationId = @ReservationId;";
        var updateQuery = "UPDATE Reservation SET AmountOfTickets = @AmountOfTickets, Used = @Used, CustomerId = @CustomerId, TheatreShowDateId = @TheatreShowDateId WHERE ReservationId = @ReservationId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        // Check if the reservation exists
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@ReservationId", reservationBody.ReservationId);
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;

            if (!exists)
            {
                throw new Exception("No reservation with the specified ID exists.");
            }
        }

        // Update the reservation
        await using (var updateCommand = new SqliteCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@AmountOfTickets", reservationBody.AmountOfTickets);
            updateCommand.Parameters.AddWithValue("@Used", reservationBody.Used);
            updateCommand.Parameters.AddWithValue("@CustomerId", reservationBody.CustomerId);
            updateCommand.Parameters.AddWithValue("@TheatreShowDateId", reservationBody.TheatreShowDateId);
            updateCommand.Parameters.AddWithValue("@ReservationId", reservationBody.ReservationId);

            await updateCommand.ExecuteNonQueryAsync();
        }
    }


    public async Task DeleteReservationAsync(int id)
    {
        var query = "DELETE FROM Reservation WHERE ReservationId = @Id;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        await using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}