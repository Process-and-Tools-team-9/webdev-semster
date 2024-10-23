using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterKit.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/ManageReservations")]
    public class ManageReservationsController : Controller
    {
        private readonly IManageReservationsService _manageReservationsService;

        public ManageReservationsController(IManageReservationsService manageReservationsService)
        {
            _manageReservationsService = manageReservationsService;
        }

        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations(int? showId, DateTime? date)
        {
            var reservations = await _manageReservationsService.GetReservationsAsync(showId, date);
            return Ok(reservations);
        }

        [HttpGet("SearchReservations")]
        public async Task<IActionResult> SearchReservations(string email, int? reservationNumber)
        {
            var reservations = await _manageReservationsService.SearchReservationsAsync(email, reservationNumber);
            return Ok(reservations);
        }

        [HttpPut("MarkReservationAsUsed/{reservationId}")]
        public async Task<IActionResult> MarkReservationAsUsed(int reservationId)
        {
            await _manageReservationsService.MarkReservationAsUsedAsync(reservationId);
            return Ok("Reservation marked as used.");
        }

        [HttpDelete("DeleteReservation/{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            await _manageReservationsService.DeleteReservationAsync(reservationId);
            return Ok("Reservation deleted.");
        }
    }
}