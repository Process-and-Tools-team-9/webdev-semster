using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;

namespace StarterKit.Controllers;

[Route("api/v1/Reservation")]

public class ReservationController : Controller
{

    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost("AddReservation")]

    public async Task<IActionResult> CreateReservation([FromBody] ReservationBody reservationBody)
    {
        if (reservationBody.AmountOfTickets == null ||
            reservationBody.Used == null ||
            reservationBody.CustomerId == null ||
            reservationBody.TheatreShowDateId == null)
        {
            return BadRequest("AmountOfTickets, Used, CustomerId, and TheatreShowDateId are required.");
        }
        try
        {
            await _reservationService.AddReservationAsync(reservationBody);
            return Ok("Reservation added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("GetReservation/{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Reservation ID is required.");
        }
        try
        {
            var reservation = await _reservationService.GetReservationAsync(id);
            return Ok(reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpDelete("DeleteReservation/{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Reservation ID is required.");
        }
        try
        {
            await _reservationService.DeleteReservationAsync(id);
            return Ok("Reservation deleted successfully.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "No reservation with the specified ID exists")
            {
                return NotFound(ex.Message);
            }
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}

public class ReservationBody
{
    public int? ReservationId { get;set; }
    public int? AmountOfTickets { get;set; }
    public int? Used { get;set; }
    public int? CustomerId { get;set; }
    public int? TheatreShowDateId { get;set; }
}