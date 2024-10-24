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


    public async Task<double> Add(ReservationBody reservationBody)
    {
        double totalPrice = 0.0;

        foreach (var showDateId in reservationBody.TheatreShowDateIds)
        {
            var showDate = await _context.TheatreShowDate
                .Include(tsd => tsd.TheatreShow)
                .ThenInclude(ts => ts.Venue)
                .FirstOrDefaultAsync(x => x.TheatreShowDateId == showDateId);

            if (showDate == null)
            {
                return -1;
            }

            if (showDate.TheatreShow.Venue.Capacity < reservationBody.amountOfTickets)
            {
                return -2;
            }

            if (showDate.DateAndTime < DateTime.Now)
            {
                return -3;
            }

            var customer = new Customer
            {
                FirstName = reservationBody.FirstName,
                LastName = reservationBody.LastName,
                Email = reservationBody.Email,
                Reservations = new List<Reservation>()
            };

            var reservation = new Reservation
            {
                AmountOfTickets = reservationBody.amountOfTickets,
                Used = false,
                Customer = customer,
                TheatreShowDate = showDate
            };

            customer.Reservations.Add(reservation);
            await _context.Customer.AddAsync(customer);
            totalPrice += showDate.TheatreShow.Price * reservationBody.amountOfTickets;
        }

        await _context.SaveChangesAsync();

        return totalPrice;
    }



    public async Task<List<Reservation>?> GetAll()
    {
        return null;
    }

    public async Task<Reservation> GetById(int id)
    {
        return null;
    }


    public async Task<bool> Update(Reservation reservation)
    {
        var resToUpdate = await _context.Reservation.Where(x => x.ReservationId == reservation.ReservationId).FirstOrDefaultAsync();
        if(resToUpdate == null)
        {
            return false;
        }
        resToUpdate = reservation;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> Delete(int id)
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