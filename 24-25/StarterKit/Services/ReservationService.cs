using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Services;

public class ReservationService : IReservationService
{
    private readonly DatabaseContext _context;

    public ReservationService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<bool> AddReservationAsync(ReservationBody reservationBody)
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

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }


#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    public async Task<List<Reservation>?> GetAllReservationsAsync()
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    {
        var reservations = await _context.Reservation.ToListAsync();
        if(reservations == null)
        {
            return null;
        }
        return reservations;
    }

    public async Task<Reservation> GetReservationAsync(int id)
    {
        var reservation = await _context.Reservation.Where(x => x.ReservationId == id).FirstOrDefaultAsync();
        if(reservation == null)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        return reservation;
    }


    public async Task<bool> UpdateReservationAsync(ReservationBody reservationBody)
    {
        var checkQuery = "SELECT COUNT(1) FROM Reservation WHERE ReservationId = @ReservationId;";
        var updateQuery = "UPDATE Reservation SET AmountOfTickets = @AmountOfTickets, Used = @Used, CustomerId = @CustomerId, TheatreShowDateId = @TheatreShowDateId WHERE ReservationId = @ReservationId;";

        await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
        await connection.OpenAsync();

        // Check if the reservation exists
        await using (var checkCommand = new SqliteCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@ReservationId", reservationBody.ReservationId);
#pragma warning disable CS8605 // Unboxing a possibly null value.
            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;
#pragma warning restore CS8605 // Unboxing a possibly null value.

            if (!exists)
            {
                return false;
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

            var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }


    public async Task<bool> DeleteReservationAsync(int id)
    {
        var resToDelete = await _context.Reservation.Where(x => x.ReservationId == id).FirstOrDefaultAsync();
        if(resToDelete == null)
        {
            return false;
        }
        _context.Reservation.Remove(resToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}