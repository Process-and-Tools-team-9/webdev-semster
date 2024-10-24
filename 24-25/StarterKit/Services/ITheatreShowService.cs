using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface ITheatreService{
    public Task<bool> Add(TheatreShowBody TheatreBody);
    public Task<Object?> GetById(int id);
    public Task<bool> Update(TheatreShow theatreShow);
    Task<bool> Delete(int id);
    Task<List<TheatreShowDto>> GetAll();
}