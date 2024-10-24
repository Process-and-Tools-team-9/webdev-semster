using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface IReservationService{
    Task<double> Add(ReservationBody reservationBody);
    Task<Reservation> GetById(int id);
    Task<bool> Update(Reservation reservation);
    Task<bool> Delete(int id);
    Task<List<Reservation>> GetAll();
}