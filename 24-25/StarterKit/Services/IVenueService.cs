using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface IVenueService{
    Task AddVenueAsync(VenueBody venueBody);
    Task<Venue> GetVenueAsync(int id);
}