using StarterKit.Controllers;
using StarterKit.Models;

namespace StarterKit.Services;

public interface ILoginService {
    public Task<LoginStatus> CheckPassword(string username, string inputPassword);
    public Task<bool> CreateAdminDB(Admin admin);
}