using Azure.Core;
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
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
       
        [HttpPost("ScheduledAppointments")]
        public IActionResult ScheduledAppointments(int clinicId, DateTime date)
        {
            try
            {
                // Extract the token from the request and validate it
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Validate user role
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return Unauthorized(new { message = "You are not authorized to perform this action." });
                }

                // Call the service to retrieve scheduled appointments
                var appointments = _bookingService.ScheduledAppointments(clinicId, date);

                // Return the result
                return Ok(new
                {
                    message = "Scheduled appointments retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                // Return a 500 response with a generic error message
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
