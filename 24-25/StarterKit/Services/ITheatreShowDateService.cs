using StarterKit.Models;
using StarterKit.Controllers;
namespace StarterKit.Services;
public interface ITheatreShowDateService
{
    Task AddTheatreShowDateAsync(_theatreShowDate theatreShowDateBody);
    Task<_theatreShowDate> GetTheatreShowDateAsync(int id);
    Task<List<_theatreShowDate>> GetAllTheatreShowDatesAsync();
    Task UpdateTheatreShowDateAsync(_theatreShowDate theatreShowDateBody);
    Task DeleteTheatreShowDateAsync(int id);
}
//TheatrShowDate zelfde naam als in de models