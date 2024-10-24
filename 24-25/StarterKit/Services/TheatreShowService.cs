using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Services;

public class TheatreService : ITheatreService
{
    private readonly DatabaseContext _context;

    public TheatreService(DatabaseContext context){
        _context = context;
    }

   public async Task<bool> Add(TheatreShowBody theatreShowBody)
    {   
        TheatreShow newTheatreShow = new TheatreShow();

        List<TheatreShowDate> ToBetheatreShowDates = new List<TheatreShowDate>(){};
        Venue venueToAdd = new Venue();

        if (theatreShowBody.Venue == null)
        {
            theatreShowBody.Venue = new Venue
            {
                Name = "Default Venue",
                Capacity = 100,
                TheatreShows = new List<TheatreShow>()
            };
        }
        else
        {
            venueToAdd = theatreShowBody.Venue;
        }

        foreach (DateTime time in theatreShowBody.Dates)
        {
            ToBetheatreShowDates.Add(new TheatreShowDate
            {
                DateAndTime = time,
                Reservations = new List<Reservation>(),
                TheatreShow = newTheatreShow
            });
        }

        newTheatreShow.Title = theatreShowBody.Title;
        newTheatreShow.Description = theatreShowBody.Description;
        newTheatreShow.theatreShowDates = ToBetheatreShowDates;
        newTheatreShow.Venue = venueToAdd;
        newTheatreShow.Price = 0;
        newTheatreShow.Venue.TheatreShows.Add(newTheatreShow);

        try
        {
            await _context.TheatreShow.AddAsync(newTheatreShow);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<object?> GetById(int id)
    {
        return await _context.TheatreShow
            .Where(x => x.TheatreShowId == id)
            .Select(ts => new {
                ts.TheatreShowId,
                ts.Title,
                ts.Description,
                ts.Price,
                Venue = new {
                    ts.Venue.VenueId,
                    ts.Venue.Name,
                    ts.Venue.Capacity
                },
                TheatreShowDates = ts.theatreShowDates.Select(d => new {
                    d.TheatreShowDateId,
                    d.DateAndTime,
                    Reservations = d.Reservations.Select(r => new {
                        r.ReservationId,
                        r.AmountOfTickets,
                        r.Used,
                        Customer = new {
                            r.Customer.CustomerId,
                            r.Customer.FirstName,
                            r.Customer.LastName,
                            r.Customer.Email
                        }
                    }).ToList()
                }).ToList()
            }).FirstOrDefaultAsync();
    }


    public async Task<List<TheatreShowDto>> GetAll()
    {
        return await _context.TheatreShow
            .Select(ts => new TheatreShowDto {
                TheatreShowId = ts.TheatreShowId,
                Title = ts.Title,
                Description = ts.Description,
                Price = ts.Price,
                Venue = new VenueDto {
                    VenueId = ts.Venue.VenueId,
                    Name = ts.Venue.Name,
                    Capacity = ts.Venue.Capacity
                },
                TheatreShowDates = ts.theatreShowDates.Select(d => new TheatreShowDateDto {
                    TheatreShowDateId = d.TheatreShowDateId,
                    DateAndTime = d.DateAndTime,
                    Reservations = d.Reservations.Select(r => new ReservationDto {
                        ReservationId = r.ReservationId,
                        AmountOfTickets = r.AmountOfTickets,
                        Used = r.Used,
                        Customer = new CustomerDto {
                            CustomerId = r.Customer.CustomerId,
                            FirstName = r.Customer.FirstName,
                            LastName = r.Customer.LastName,
                            Email = r.Customer.Email
                        }
                    }).ToList()
                }).ToList()
            }).ToListAsync();
    }

    public async Task<bool> Update(TheatreShow theatreShow)
    {
        var theatreShowToUpdate = await _context.TheatreShow.Where(x => x.TheatreShowId == theatreShow.TheatreShowId).FirstOrDefaultAsync();
        if (theatreShowToUpdate == null)
        {
            return false;
        }
        theatreShowToUpdate = theatreShow;
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var theatreShowToDelete = await _context.TheatreShow.Include(ts => ts.Venue).Where(x => x.TheatreShowId == id).FirstOrDefaultAsync();
        if (theatreShowToDelete == null)
        {
            return false;
        }
        if (theatreShowToDelete.Venue != null)
        {
            theatreShowToDelete.Venue.TheatreShows.Remove(theatreShowToDelete);
        }
        _context.TheatreShow.Remove(theatreShowToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class TheatreShowDto
{
    public int TheatreShowId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public VenueDto Venue { get; set; }
    public List<TheatreShowDateDto> TheatreShowDates { get; set; }
}

public class VenueDto
{
    public int VenueId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
}

public class TheatreShowDateDto
{
    public int TheatreShowDateId { get; set; }
    public DateTime DateAndTime { get; set; }
    public List<ReservationDto> Reservations { get; set; }
}

public class ReservationDto
{
    public int ReservationId { get; set; }
    public int AmountOfTickets { get; set; }
    public bool Used { get; set; }
    public CustomerDto Customer { get; set; }
}

public class CustomerDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
