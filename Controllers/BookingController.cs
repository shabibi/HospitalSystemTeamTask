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

        [HttpPost("BookAppointment")]
        public IActionResult BookAppointment(BookingInputDTO bookingInput)
        {
            try
            {
                // Extract the token from the request and retrieve the user's role
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                // Check if the user's role allows them to perform this action
                if (userRole == null || userRole != "patient")
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }
                _bookingService.BookAppointment(bookingInput, userId);
                return Ok(" Appointment booked successfully");
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while booking appointment : {ex.Message}");
            }
        }
        /// <summary>
        /// Get all bookings.
        /// </summary>
        /// <returns>A list of all bookings.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<BookingOutputDTO>> GetAllBookings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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

                // get all bookings
                var bookings = _bookingService.GetAllBooking(pageNumber,pageSize);

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with the error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching bookings.", Details = ex.Message });
            }
        }
    }
}
