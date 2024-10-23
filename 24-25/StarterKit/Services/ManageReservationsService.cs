using StarterKit.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterKit.Services
{
    public class ManageReservationsService : IManageReservationsService
    {
        private readonly DatabaseContext _context;

        public ManageReservationsService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetReservationsAsync(int? showId, DateTime? date)
        {
            var query = "SELECT * FROM Reservation WHERE 1=1";
            if (showId.HasValue)
            {
                query += " AND TheatreShowDateId = @ShowId";
            }
            if (date.HasValue)
            {
                query += " AND DATE(ReservationDate) = DATE(@Date)";
            }

            var reservations = new List<Reservation>();

            await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
            await connection.OpenAsync();

            await using var command = new SqliteCommand(query, connection);
            if (showId.HasValue)
            {
                command.Parameters.AddWithValue("@ShowId", showId.Value);
            }
            if (date.HasValue)
            {
                command.Parameters.AddWithValue("@Date", date.Value);
            }

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var reservation = new Reservation
                {
                    ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId"))
                };
                reservations.Add(reservation);
            }

            return reservations;
        }

        public async Task<List<Reservation>> SearchReservationsAsync(string email, int? reservationNumber)
        {
            var query = "SELECT * FROM Reservation WHERE 1=1";
            if (!string.IsNullOrEmpty(email))
            {
                query += " AND Email = @Email";
            }
            if (reservationNumber.HasValue)
            {
                query += " AND ReservationId = @ReservationNumber";
            }

            var reservations = new List<Reservation>();

            await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
            await connection.OpenAsync();

            await using var command = new SqliteCommand(query, connection);
            if (!string.IsNullOrEmpty(email))
            {
                command.Parameters.AddWithValue("@Email", email);
            }
            if (reservationNumber.HasValue)
            {
                command.Parameters.AddWithValue("@ReservationNumber", reservationNumber.Value);
            }

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var reservation = new Reservation
                {
                    ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                };
                reservations.Add(reservation);
            }

            return reservations;
        }

        public async Task MarkReservationAsUsedAsync(int reservationId)
        {
            var query = "UPDATE Reservation SET IsUsed = 1 WHERE ReservationId = @ReservationId";

            await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
            await connection.OpenAsync();

            await using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservationId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var query = "DELETE FROM Reservation WHERE ReservationId = @ReservationId";

            await using var connection = new SqliteConnection(@"Data Source=webdev.sqlite;");
            await connection.OpenAsync();

            await using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ReservationId", reservationId);

            await command.ExecuteNonQueryAsync();
        }
    }
}