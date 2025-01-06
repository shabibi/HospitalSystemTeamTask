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
            catch (InvalidOperationException ex)
            {
                // Return a specific error response
                return BadRequest(new { message = ex.Message });
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

        [HttpGet("availableAppointments")]
        public IActionResult GetAvailableAppointmentsBy(int? clinicId, int? departmentId)
        {
            try
            {
                // Validate that only one parameter (clinicId or departmentId) is provided
                if (clinicId.HasValue && departmentId.HasValue)
                    throw new ArgumentException("Only one parameter can be provided at a time.");

                // Validate that at least one parameter is provided
                if (!clinicId.HasValue && !departmentId.HasValue)
                    throw new ArgumentException("At least one parameter must be provided.");

                // Call the service method to get available appointments
                var availableAppointments = _bookingService.GetAvailableAppointmentsBy(clinicId, departmentId);

                // Return the result as an OkResponse with the available appointments
                return Ok(availableAppointments);
            }
            catch (InvalidOperationException ex)
            {
                // Handle no available bookings error and return a BadRequest response with the error message
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any errors and return a BadRequest response with the error message
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("bookedAppointments")]
        public IActionResult GetBookedAppointments(
        [FromQuery] int? patientId,
        [FromQuery] int? clinicId,
        [FromQuery] int? departmentId,
        [FromQuery] DateTime? date)
        {
            try
            {
                // Ensure only one parameter is provided
                int providedParametersCount = new List<object> { patientId, clinicId, departmentId, date }.Count(p => p != null);
                if (providedParametersCount > 1)
                {
                    throw new ArgumentException("Only one parameter can be provided at a time.");
                }

                // Extract the token from the request and validate it
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                int loggedInUserId = int.Parse(JwtHelper.GetClaimValue(token, "sub")); // Assuming "sub" holds the user ID

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing or invalid." });
                }

                // Check if the user has sufficient authorization
                if (string.IsNullOrEmpty(userRole))
                {
                    return Unauthorized(new { message = "You are not authorized to perform this action." });
                }

                // Call the service method to get booked appointments based on the provided parameters
                var bookedAppointments = _bookingService.GetBookedAppointments(patientId, clinicId, departmentId, date);

                // If the user is a patient, filter appointments to show only theirs
                if (userRole == "patient" && bookedAppointments != null)
                {  
                    // Check if the provided patientId is the same as the logged-in patient
                    if (patientId.HasValue && patientId.Value != loggedInUserId)
                    {
                        return Unauthorized(new { message = "You are not authorized to view another patient's appointments." });
                    }

                    // Filter appointments to only show the logged-in patient's appointments if no patientId is provided
                    if (patientId == null)
                    {
                        bookedAppointments = bookedAppointments.Where(a => a.PID == loggedInUserId).ToList();
                    }
                }

                // Return the result as an OkResponse with the booked appointments
                return Ok(bookedAppointments);
            }
            catch (InvalidOperationException ex)
            {
                // Handle errors such as no appointments found and return a BadRequest with the error message
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("cancelAppointment")]
        public IActionResult CancelAppointment([FromBody] BookingInputDTO bookingInputDTO)
        {
            try
            {
                // Extract the token from the request
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                int userId = int.Parse(JwtHelper.GetClaimValue(token, "sub")); // Assuming "sub" holds the user's ID

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing or invalid." });
                }

                // Retrieve the appointment to validate permissions
                var appointment = _bookingService
                    .GetBookingsByClinicAndDate(bookingInputDTO.CID, bookingInputDTO.Date)
                    .FirstOrDefault(b => b.StartTime == bookingInputDTO.StartTime);

                if (appointment == null)
                {
                    return NotFound(new { message = "Appointment not found for the provided details." });
                }

                // Authorization checks
                if (userRole == "patient")
                {
                    // Ensure the patient can only cancel their own appointment
                    if (appointment.PID != userId)
                    {
                        return Unauthorized(new { message = "You are not authorized to cancel this appointment." });
                    }
                }
                else if (userRole != "admin" && userRole != "superAdmin" && userRole != "doctor")
                {
                    // Restrict cancellation for non-admin/non-doctor roles
                    return Unauthorized(new { message = "You do not have sufficient permissions to cancel this appointment." });
                }

                // Call the service to cancel the appointment
                _bookingService.CancelAppointment(bookingInputDTO, appointment.PID.Value);

                return Ok(new { message = "Appointment successfully canceled." });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("updateAppointment")]
        public IActionResult UpdateAppointment([FromBody] UpdateBookingDTO updateAppointmentDTO)
        {
            try
            {
                // Extract the patient ID from the token
                string token = JwtHelper.ExtractToken(Request);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing or invalid." });
                }

                int patientId = int.Parse(JwtHelper.GetClaimValue(token, "sub")); // Assuming "sub" holds the patient's ID

                // Call the service to update the appointment
                _bookingService.UpdateBookedAppointment(updateAppointmentDTO.PreviousAppointment, updateAppointmentDTO.NewAppointment, patientId);

                return Ok(new { message = "Appointment successfully updated." });
            }
            catch (Exception ex)
            {
                // Handle different exceptions and provide meaningful feedback
                if (ex.Message.Contains("No appointment found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                if (ex.Message.Contains("cannot update for another patient"))
                {
                    return Unauthorized(new { message = ex.Message });
                }
                if (ex.Message.Contains("already booked") || ex.Message.Contains("not currently booked"))
                {
                    return BadRequest(new { message = ex.Message });
                }

                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("deleteAppointment")]
        public IActionResult DeleteAppointment([FromBody] BookingInputDTO bookingInputDTO)
        {
            try
            {
                // Extract the patient ID from the token
                string token = JwtHelper.ExtractToken(Request);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing or invalid." });
                }

                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                // Validate user role
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return Unauthorized(new { message = "You are not authorized to perform this action." });
                }

                // Call the service to delete the appointment
                _bookingService.DeleteAppointments(bookingInputDTO);

                return Ok(new { message = "Appointment successfully deleted." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }


}



