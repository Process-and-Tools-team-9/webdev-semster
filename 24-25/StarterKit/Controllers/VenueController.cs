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


    [HttpPost("AddVenue")]
    public async Task<IActionResult> CreateVenue([FromBody] VenueBody venueBody)
    {
        if (string.IsNullOrEmpty(venueBody.Name) || 
            string.IsNullOrEmpty(venueBody.Capacity.ToString()))
        {
            return BadRequest("Name and Capacity are required.");
        }

        try
        {
            // Call the asynchronous service method
            await _venueService.AddVenueAsync(venueBody);
            return Ok("Venue added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpGet("GetVenue/{id}")]
    public async Task<IActionResult> GetVenue(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Venue ID is required.");
        }
        try
        {
            // Call the asynchronous service method
            var venue = await _venueService.GetVenueAsync(id);
            return Ok(venue);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpPut("UpdateVenue")]
    public async Task<IActionResult> UpdateVenue([FromBody] VenueBody venueBody)
    {
        if (venueBody.VenueId == null || string.IsNullOrEmpty(venueBody.Name) || 
            venueBody.Capacity == null)
        {
            return BadRequest("VenueId, Name, and Capacity are required.");
        }

        try
        {
            await _venueService.UpdateVenueAsync(venueBody);
            return Ok("Venue updated successfully.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "No venue with the specified ID exists.")
            {
                return NotFound(ex.Message);
            }
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpDelete("DeleteVenue/{id}")]
    public async Task<IActionResult> DeleteVenue(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Venue ID is required.");
        }
        try
        {
            var venue = await _venueService.GetVenueAsync(id);
            if (venue == null)
            {
                return NotFound("No venue with the specified ID exists.");
            }
            // Call the asynchronous service method
            await _venueService.DeleteVenueAsync(id);
            return Ok("Venue deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}



public class VenueBody
{
    public int? VenueId { get; set; }
    public string? Name { get; set; }
    public int? Capacity { get; set; }
}