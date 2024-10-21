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
        if(!await _reservationService.AddReservationAsync(reservationBody))
        {
            return BadRequest("Error adding reservation.");
        }
        return Ok("Reservation added successfully.");
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Reservation ID is required and must be greater than zero.");
        }
        var reservation = await _reservationService.GetReservationAsync(id);
        if (reservation == null)
        {
            return NotFound("No reservation with the specified ID exists.");
        }
        return Ok(reservation);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        if (reservations == null)
        {
            return NotFound("No reservations found.");
        }
        return Ok(reservations);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateReservation([FromBody] ReservationBody reservationBody)
    {
        if (reservationBody.ReservationId == 0 || reservationBody.AmountOfTickets <= 0 || 
            reservationBody.CustomerId <= 0 || reservationBody.TheatreShowDateId <= 0)
        {
            return BadRequest("ReservationId, AmountOfTickets, CustomerId, and TheatreShowDateId are required and must be greater than zero.");
        }

        if(!await _reservationService.UpdateReservationAsync(reservationBody))
        {
            return NotFound("No reservation with the specified ID exists.");
        }
        return Ok("Reservation updated successfully.");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Reservation ID is required and must be greater than zero.");
        }
        if(!await _reservationService.DeleteReservationAsync(id))
        {
            return NotFound("No reservation with the specified ID exists.");
        }
        return Ok("Reservation deleted successfully.");
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