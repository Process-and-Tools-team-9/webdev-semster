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


    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationBody reservationBody)
    {
        if (reservationBody == null || 
            string.IsNullOrEmpty(reservationBody.FirstName) || 
            string.IsNullOrEmpty(reservationBody.LastName) || 
            string.IsNullOrEmpty(reservationBody.Email))
        {
            return BadRequest("First name, last name, email, and reservation details are required.");
        }

        if (reservationBody.amountOfTickets <= 0)
        {
            return BadRequest("Amount of tickets must be greater than zero.");
        }

        try
        {
            double result = await _reservationService.Add(reservationBody);

            if (result == -1)
            {
                return BadRequest("One or more Theatre show date IDs are invalid.");
            }
            if (result == -2)
            {
                return BadRequest("Not enough capacity for one or more selected shows.");
            }
            if (result == -3)
            {
                return BadRequest("One or more selected shows have already finished.");
            }

            return Ok(new { totalPrice = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the reservation: " + ex.Message);
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        return BadRequest("Not Implemented");
    }


    [HttpGet]
    public async Task<IActionResult> GetAllReservations()
    {
        return BadRequest("Not Implemented");
    }


    [HttpPut]
    public async Task<IActionResult> UpdateReservation([FromBody] Reservation reservation)
    {
        if (reservation == null || reservation.ReservationId <= 0)
        {
            return BadRequest("Reservation ID is required and must be greater than zero.");
        }
        if(!await _reservationService.Update(reservation))
        {
            return BadRequest("Error updating reservation.");
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
        if(!await _reservationService.Delete(id))
        {
            return NotFound("No reservation with the specified ID exists.");
        }
        return Ok("Reservation deleted successfully.");
    }
}


public class ReservationBody
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int amountOfTickets { get; set; }
    public List<int> TheatreShowDateIds { get; set; }
}                                                                                                          