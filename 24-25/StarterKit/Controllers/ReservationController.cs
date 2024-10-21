using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
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
        if (reservationBody == null || 
            reservationBody.AmountOfTickets <= 0 || 
            reservationBody.CustomerId <= 0 || 
            reservationBody.TheatreShowDateId <= 0)
        {
            return BadRequest("AmountOfTickets, CustomerId, and TheatreShowDateId are required and must be greater than zero.");
        }

        try
        {
            // Call the asynchronous service method
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
            return BadRequest("Reservation ID is required and must be greater than zero.");
        }
        try
        {
            // Call the asynchronous service method
            var reservation = await _reservationService.GetReservationAsync(id);
            return Ok(reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpGet("GetReservation")]
    public async Task<IActionResult> GetAllReservations()
    {
        try
        {
            // Call the asynchronous service method
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpPut("UpdateReservation")]
    public async Task<IActionResult> UpdateReservation([FromBody] ReservationBody reservationBody)
    {
        if (reservationBody.ReservationId == 0 || reservationBody.AmountOfTickets <= 0 || 
            reservationBody.CustomerId <= 0 || reservationBody.TheatreShowDateId <= 0)
        {
            return BadRequest("ReservationId, AmountOfTickets, CustomerId, and TheatreShowDateId are required and must be greater than zero.");
        }

        try
        {
            await _reservationService.UpdateReservationAsync(reservationBody);
            return Ok("Reservation updated successfully.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "No reservation with the specified ID exists.")
            {
                return NotFound(ex.Message);
            }
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpDelete("DeleteReservation/{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Reservation ID is required and must be greater than zero.");
        }
        try
        {
            var reservation = await _reservationService.GetReservationAsync(id);
            if (reservation == null)
            {
                return NotFound("No reservation with the specified ID exists.");
            }
            // Call the asynchronous service method
            await _reservationService.DeleteReservationAsync(id);
            return Ok("Reservation deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}


public class ReservationBody
{
    public int ReservationId { get; set; }

    public int AmountOfTickets { get; set; }

    public bool Used { get; set; }

    public int CustomerId { get; set; }
    public int TheatreShowDateId { get; set; }
}                                                                                                          