using StarterKit.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterKit.Services
{
    public interface IManageReservationsService
    {
        Task<List<Reservation>> GetReservationsAsync(int? showId, DateTime? date);
        Task<List<Reservation>> SearchReservationsAsync(string email, int? reservationNumber);
        Task MarkReservationAsUsedAsync(int reservationId);
        Task DeleteReservationAsync(int reservationId);
    }
}