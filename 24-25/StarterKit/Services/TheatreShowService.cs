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

        if (theatreShowBody.venue == null)
        {
            theatreShowBody.venue = new Venue
            {
                Name = "Default Venue",
                Capacity = 100,
                TheatreShows = new List<TheatreShow>()
            };
        }
        else
        {
            venueToAdd = theatreShowBody.venue;
        }

        foreach (DateTime time in theatreShowBody.dates)
        {
            ToBetheatreShowDates.Add(new TheatreShowDate
            {
                DateAndTime = time,
                Reservations = new List<Reservation>(),
                TheatreShow = newTheatreShow
            });
        }

        newTheatreShow.Title = theatreShowBody.Title;
        newTheatreShow.Description = theatreShowBody.description;
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


    public async Task<TheatreShow> GetById(int id)
    {
        var theatreShow = await _context.TheatreShow
        .Include(ts => ts.Venue)
        .FirstOrDefaultAsync(x => x.TheatreShowId == id);
        return theatreShow;
    }


    public async Task<List<TheatreShow>> GetAll()
    {
        var theatreShows = await _context.TheatreShow.ToListAsync();
        if (theatreShows == null)
        {
            return new List<TheatreShow>(){};
        }
        return theatreShows;
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
