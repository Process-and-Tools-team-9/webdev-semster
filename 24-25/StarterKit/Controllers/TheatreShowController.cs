using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;


namespace StarterKit.Controllers;

[Route("api/v1/TheatreShow")]

public class TheatreController : Controller{
    private readonly ITheatreService _theatreService;

    public TheatreController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTheatreShow([FromBody] TheatreShowBody theatreShowBody)
    {
        if(!IsAdminLoggedInCheck())
        {
            return Unauthorized("Admin is not logged in.");
        }
        if(theatreShowBody.Dates == null || theatreShowBody.Dates.Count == 0)
        {
            return BadRequest("Dates are required.");
        }
        if(theatreShowBody.Title == null)
        {
            return BadRequest("Title is required.");
        }
        if(theatreShowBody.Description == null)
        {
            return BadRequest("Description is required.");
        }
        if(!await _theatreService.Add(theatreShowBody))
        {
            return BadRequest("TheatreShow could not be added.");
        }
        return Ok("TheatreShow added successfully.");
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetTheatre(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Theatreshow ID is required.");
        }
        try
        {
            var Theatre = await _theatreService.GetById(id);
            if(Theatre == null)
            {
                return NotFound("No TheatreShow with the specified ID exists.");
            }
            return Ok(Theatre);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "There was an error with adding the theatre: " + ex.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        try
        {
            var Theatres = await _theatreService.GetAll();
            if(Theatres == null)
            {
                return NotFound("No Theatres exist.");
            }
            return Ok(Theatres);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "There was an error with retrieving the Theatres: " + ex.Message);
        }
    }


    [HttpPut]
    public async Task<IActionResult> UpdateTheatre([FromBody] TheatreShow theatreShow)
    {
        if(!IsAdminLoggedInCheck())
        {
            return Unauthorized("Admin is not logged in.");
        }
        if(theatreShow == null)
        {
            return BadRequest("Valid TheatreShow is required.");
        }
        try
        {
            if(!await _theatreService.Update(theatreShow))
            {
                return NotFound("No TheatreShow with the specified ID exists.");
            }
            return Ok("TheatreShow updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"There was an error with updating the TheatreShow, {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        if(!IsAdminLoggedInCheck())
        {
            return Unauthorized("Admin is not logged in.");
        }
        if(id <= 0)
        {
            return BadRequest("Valid Theatre ID is required.");
        }
        try
        {
            if(!await _theatreService.Delete(id))
            {
                return NotFound("No Theatre with the specified ID exists.");
            }
            return Ok("Theatre deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500,$"There was an error with deleting the Theatre, {ex.Message}");
        }
    }

    public bool IsAdminLoggedInCheck()
    {
        string username = HttpContext.Session.GetString(ADMIN_SESSION_KEY.adminLoggedIn.ToString());
        return username != null;
    }
}

public class TheatreShowBody
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public Venue? Venue { get; set; }
    public List<DateTime>? Dates { get; set; }
}