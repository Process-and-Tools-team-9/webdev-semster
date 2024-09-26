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

    [HttpPost("/AddCustomer")]
public async Task<IActionResult> CreateCustomer([FromBody] CustomerBody customerBody)
{
    if (string.IsNullOrEmpty(customerBody.FirstName) || 
        string.IsNullOrEmpty(customerBody.LastName) || 
        string.IsNullOrEmpty(customerBody.Email))
    {
        return BadRequest("Username, Password, and Email are required.");
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
}

public class CustomerBody{
    public int? CustomerId;
    public string? FirstName;
    public string? LastName;
    public string? Email;
}