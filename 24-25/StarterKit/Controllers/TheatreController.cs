using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;


namespace StarterKit.Controllers;

public class TheatreController : Controller{
    private readonly ITheatreService _TheatreService;

    public TheatreController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    [HttpPost("AddTheatre")]
    public async Task<IActionResult> CreateTheatre([FromBody] Theatre TheatreBody)
    {
        if (string.IsNullOrEmpty(TheatreBody.Title) || 
            string.IsNullOrEmpty(TheatreBody.Description))
        {
            return BadRequest("Title and Description are required.");
        }

        try
        {
            // Call the asynchronous service method
            await _TheatreService.AddTheatreAsync(TheatreBody);
            return Ok("Theatre added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpGet("GetTheatreShow/{id}")]
    public async Task<IActionResult> GetTheatre(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Theatreshow ID is required.");
        }
        try
        {
            // Call the asynchronous service method
            var Theatre = await _TheatreService.GetTheatreAsync(id);
            return Ok(Theatre);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpGet("GetTheatre")]
    public async Task<IActionResult> GetAllTheatres()
    {
        try
        {
            // Call the asynchronous service method
            var Theatres = await _TheatreService.GetAllTheatreAsync();
            return Ok(Theatres);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpPut("UpdateTheatre")]
    public async Task<IActionResult> UpdateTheatre([FromBody] Theatre TheatreBody)
    {
        if (TheatreBody.TheatreShowId == null || string.IsNullOrEmpty(TheatreBody.Title))
        {
            return BadRequest("TheatreShowId, Title are required.");
        }

        try
        {
            await _TheatreService.UpdateTheatreAsync(TheatreBody);
            return Ok("Theatre updated successfully.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "No Theatre with the specified ID exists.")
            {
                return NotFound(ex.Message);
            }
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpDelete("DeleteTheatreShow/{id}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Theatre ID is required.");
        }
        try
        {
            var Theatre = await _TheatreService.GetTheatreAsync(id);
            if (Theatre == null)
            {
                return NotFound("No Theatre with the specified ID exists.");
            }
            // Call the asynchronous service method
            await _TheatreService.DeleteTheatreAsync(id);
            return Ok("Theatre deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}

public class Theatre
{
    public int TheatreShowId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }
}