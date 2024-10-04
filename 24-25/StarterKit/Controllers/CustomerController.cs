using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;


namespace StarterKit.Controllers;

[Route("api/v1/Customer")]


public class CustomerController : Controller{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

[HttpPost("AddCustomer")]
public async Task<IActionResult> CreateCustomer([FromBody] CustomerBody customerBody)
{

    if (string.IsNullOrEmpty(customerBody.FirstName) || 
        string.IsNullOrEmpty(customerBody.LastName) || 
        string.IsNullOrEmpty(customerBody.Email))
    {
        return BadRequest("FirstName, LastName, and Email are required.");
    }

    try
    {
        // Call the asynchronous service method
        await _customerService.AddCustomerAsync(customerBody);
        return Ok("Customer added successfully.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}
[HttpGet("GetCustomer/{id}")]
    public async Task<IActionResult> GetVenue(int id)
    {
        if(id <= 0)
        {
            return BadRequest($"Customer ID is required.");
        }
        try
        {
            // Call the asynchronous service method
            var customer = await _customerService.GetCustomerAsync(id);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}


public class CustomerBody{
    public int? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}