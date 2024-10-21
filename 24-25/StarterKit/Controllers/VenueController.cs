using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;


namespace StarterKit.Controllers;

[Route("api/v1/Venue")]


public class VenueController : Controller{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateVenue([FromBody] VenueBody venueBody)
    {
        if (string.IsNullOrEmpty(venueBody.Name) || 
            string.IsNullOrEmpty(venueBody.Capacity.ToString()))
        {
            return BadRequest("Name and Capacity are required.");
        }
        if(!await _venueService.AddVenueAsync(venueBody))
        {
            return BadRequest("Error adding venue.");
        }
        return Ok("Venue added successfully.");
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetVenue(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Venue ID is required.");
        }
        var venue = await _venueService.GetVenueAsync(id);
        if(venue == null)
        {
            return NotFound("No venue with the specified ID exists.");
        }
        return Ok(venue);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllVenues()
    {
        var venues = await _venueService.GetAllVenuesAsync();
        if(venues == null)
        {
            return NotFound("No venues found.");
        }
        return Ok(venues);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateVenue([FromBody] VenueBody venueBody)
    {
        if (venueBody.VenueId == null || string.IsNullOrEmpty(venueBody.Name) || 
            venueBody.Capacity == null)
        {
            return BadRequest("VenueId, Name, and Capacity are required.");
        }
        if(!await _venueService.UpdateVenueAsync(venueBody))
        {
            return NotFound("No venue with the specified ID exists.");
        }
        return Ok("Venue updated successfully.");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenue(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Venue ID is required.");
        }
        if(!await _venueService.DeleteVenueAsync(id))
        {
            return NotFound("No venue with the specified ID exists.");
        }
        return Ok("Venue deleted successfully.");
    }
}



public class VenueBody
{
    public int? VenueId { get; set; }
    public string? Name { get; set; }
    public int? Capacity { get; set; }
}