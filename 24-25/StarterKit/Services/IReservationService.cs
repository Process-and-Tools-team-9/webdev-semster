using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface IReservationService{
    Task<bool> AddReservationAsync(ReservationBody reservationBody);
    Task<Reservation> GetReservationAsync(int id);
    Task<bool> UpdateReservationAsync(ReservationBody reservationBody);
    Task<bool> DeleteReservationAsync(int id);
    Task<List<Reservation>> GetAllReservationsAsync();
}