using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface IReservationService{
    Task AddReservationAsync(ReservationBody reservationBody);
    Task<Reservation> GetReservationAsync(int id);
    Task UpdateReservationAsync(ReservationBody reservationBody);
    Task DeleteReservationAsync(int id);
    Task<List<Reservation>> GetAllReservationsAsync();
}