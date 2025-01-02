using Azure.Core;
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        public IActionResult ScheduledAppointments(int clinicId, DateTime date)
        {
            try
            {
                //// Extract the token from the request and retrieve the user's role
                //string token = JwtHelper.ExtractToken(Request);
                //var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                //// Check if the user's role allows them to perform this action
                //if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
                //{
                //    return BadRequest(new { message = "You are not authorized to perform this action." });
                //}
                _bookingService.ScheduledAppointments(clinicId, date);
                return Ok("Scheduled Appointments successfully");
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the new Branch: {ex.Message}");
            }
        }
    }
}
