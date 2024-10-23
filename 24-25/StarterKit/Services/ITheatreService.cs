using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface ITheatreService{
    Task AddTheatreAsync(Theatre TheatreBody);
    Task<Theatre> GetTheatreAsync(int id);
    Task UpdateTheatreAsync(Theatre TheatreBody);
    Task DeleteTheatreAsync(int id);
    Task<List<Theatre>> GetAllTheatreAsync();
}