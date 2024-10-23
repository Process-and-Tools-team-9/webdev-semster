using Microsoft.EntityFrameworkCore;
using StarterKit.Controllers;
using StarterKit.Models;
using StarterKit.Utils;

namespace StarterKit.Services;

public enum LoginStatus { IncorrectPassword, IncorrectUsername, Success }

public enum ADMIN_SESSION_KEY { adminLoggedIn }

public class LoginService : ILoginService
{

    private readonly DatabaseContext _context;

    public LoginService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAdminDB(Admin admin)
    {
        if (await _context.Admin.AnyAsync(a => a.UserName == admin.UserName))
        {
            return false;
        }
        if (await _context.Admin.AnyAsync(a => a.Email == admin.Email))
        {
            return false;
        }
        await _context.Admin.AddAsync(admin);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<LoginStatus> CheckPassword(string username, string inputPassword)
    {
        if (username == null || inputPassword == null)
        {
            return LoginStatus.IncorrectUsername;
        }

        var admin = await _context.Admin.SingleOrDefaultAsync(a => a.UserName == username);
        
        if (admin == null)
        {
            return LoginStatus.IncorrectUsername;
        }
        if (admin.Password == EncryptionHelper.EncryptPassword(inputPassword))
        {
            return LoginStatus.Success;
        }
        
        return LoginStatus.IncorrectPassword;
    }
}