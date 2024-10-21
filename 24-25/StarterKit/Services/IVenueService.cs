using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface IVenueService{
    Task<bool> AddVenueAsync(VenueBody venueBody);
    Task<Venue> GetVenueAsync(int id);
    Task<bool> UpdateVenueAsync(VenueBody venueBody);
    Task<bool> DeleteVenueAsync(int id);
    Task<List<Venue>> GetAllVenuesAsync();
}