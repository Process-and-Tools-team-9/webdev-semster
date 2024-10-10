using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface ITheatreService{
    Task AddTheatreAsync(Theatre theatreBody);
    Task<Theatre> GetTheatreAsync(int id);
    Task UpdateTheatreAsync(Theatre theatreBody);
    Task DeleteTheatreAsync(int id);
    Task<List<Theatre>> GetAllTheatreAsync();
}