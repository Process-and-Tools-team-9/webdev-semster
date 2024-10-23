using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;
namespace StarterKit.Controllers;
[Route("api/v1/TheatreShowDate")]
public class TheateShowDateController : Controller
{
    private readonly ITheatreShowDateService _theatreShowDateService;
    public TheateShowDateController(ITheatreShowDateService theatreShowDateService)
    {
        _theatreShowDateService = theatreShowDateService;
    }
    [HttpPost("AddTheatreShowDate")]
    public async Task<IActionResult> CreateTheatreShowDate([FromBody] _theatreShowDate TheatreShowDateBody)
    {
        if (string.IsNullOrEmpty(TheatreShowDateBody.DateAndTime) ||
            int.IsNegative(TheatreShowDateBody.TheatreShowId))
        {
            return BadRequest("Date and Time are required.");
        }
        try
        {
            await _theatreShowDateService.AddTheatreShowDateAsync(TheatreShowDateBody);
            return Ok("Theatre Show Date added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    [HttpGet("GetTheatreShowDate/{id}")]
    public async Task<IActionResult> GetTheatreShowDate(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Theatre Show Date ID is required.");
        }
        try
        {
            var _theatreShowDate = await _theatreShowDateService.GetTheatreShowDateAsync(id);
            return Ok(_theatreShowDate);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    [HttpGet("GetTheatreShowDate")]
    public async Task<IActionResult> GetAllTheatreShowDates()
    {
        try
        {
            var TheatreShowDates = await _theatreShowDateService.GetAllTheatreShowDatesAsync();
            return Ok(TheatreShowDates);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }   
    }
    [HttpPut("UpdateTheatreShowDate")]
    public async Task<IActionResult> UpdateTheatreShowDate([FromBody] _theatreShowDate TheatreShowDateBody)
    {
        if (int.IsNegative(TheatreShowDateBody.TheatreShowDateId))
        {
            return BadRequest("Theatre Show Date ID is required.");
        }
        try
        {
            await _theatreShowDateService.UpdateTheatreShowDateAsync(TheatreShowDateBody);
            return Ok("Theatre Show Date updated successfully.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "No Theatre Show Date with the specified ID exists.")
            {
                return NotFound(ex.Message);
            }
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    [HttpDelete("DeleteTheatreShowDate/{id}")]
    public async Task<IActionResult> DeleteTheatreShowDate(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Theatre Show Date ID is required.");
        }
        try
        {
            var _theatreShowDate = await _theatreShowDateService.GetTheatreShowDateAsync(id);
            if (_theatreShowDate == null)
            {
                return NotFound("No Theatre Show Date with the specified ID exists.");
            }
            await _theatreShowDateService.DeleteTheatreShowDateAsync(id);
            return Ok("Theatre Show Date deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
public class _theatreShowDate
{
    public int TheatreShowDateId { get;set; }
    public string? DateAndTime { get;set; }
    public int TheatreShowId { get;set; }
}