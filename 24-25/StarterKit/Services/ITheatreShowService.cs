using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface ITheatreService{
    public Task<bool> Add(TheatreShowBody TheatreBody);
    public Task<TheatreShow> GetById(int id);
    public Task<bool> Update(TheatreShow theatreShow);
    Task<bool> Delete(int id);
    Task<List<TheatreShow>> GetAll();
}