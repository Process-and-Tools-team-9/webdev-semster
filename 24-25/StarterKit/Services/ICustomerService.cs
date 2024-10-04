using StarterKit.Models;
using StarterKit.Controllers;

namespace StarterKit.Services;

public interface ICustomerService{
    Task AddCustomerAsync(CustomerBody customerBody);
    Task<Customer> GetCustomerAsync(int id);
}