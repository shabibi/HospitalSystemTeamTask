using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HospitalSystemTeamTask.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorServicee;
        private readonly IConfiguration _configuration;

        public DoctorController(IDoctorService doctorService, IConfiguration configuration)
        {
            _doctorServicee = doctorService;
            _configuration = configuration;
        }

        //[HttpGet("GetDoctorByEmail")]
        //public IActionResult GetDoctorByEmail(string email)
        //{
        //    try
        //    {
        //        var doctor = _doctorServicee.GetDoctorByEmail(email);
        //        if (doctor == null)
        //        {
        //            return NotFound("Doctor not found.");
        //        }

        //        return Ok(doctor);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("GetDoctorByName")]
        //public IActionResult GetDoctorByName(string docName)
        //{
        //    try
        //    {
        //        var doctor = _doctorServicee.GetDoctorByName(docName);
        //        if (doctor == null)
        //        {
        //            return NotFound("Doctor not found.");
        //        }

        //        return Ok(doctor);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [HttpGet("GetDoctor")]
        public IActionResult GetDoctor(int? DocID, string? DocName)
        {
            try
            {

                // Validate the user ID
                if (DocID < 0)
                    return BadRequest("Invalid input");

                // Validate input: At least one parameter must be provided
                if (!DocID.HasValue && string.IsNullOrWhiteSpace(DocName))
                    return BadRequest(new { message = "Invalid input. Provide either UserID or UserName." });

                // Validate input: Ensure only one field is used for search
                if (DocID.HasValue && !string.IsNullOrWhiteSpace(DocName))
                    return BadRequest(new { message = "Invalid input. Provide only one field (UserID or UserName) to search." });

                var doctor = _doctorServicee.GetDoctorData(DocName, DocID);




                return Ok(doctor);
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if the user is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving user. {(ex.Message)}");
            }
        }
        [HttpPost("AddDoctor")]
        public IActionResult AddDoctor( DoctorOutPutDTO input)
        {
            try
            {

                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null || (userRole != "admin" && userRole != "supperAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }
                if (input == null || input.UID <= 0)
                    return BadRequest("Invalid input. Doctor information and a valid ID are required.");

                // Add doctor using the service
                _doctorServicee.AddDoctor(input);

                return Ok(new { message = "Doctor added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while adding the doctor. {ex.Message}" });
            }
        }

        [HttpGet("GetDoctorsByBranch")]
        public IActionResult GetDoctorsByBranch(string branchName)
        {
            try
            {
                var doctors = _doctorServicee.GetDoctorsByBranchName(branchName);
                return Ok(doctors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = $"No doctor in this branch. {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving doctors. {ex.Message}" });
            }
        }

        [HttpGet("GetDoctorsByDepartmentName")]
        public ActionResult<IEnumerable<DoctorOutPutDTO>> GetDoctorsByDepartmentName( string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                return BadRequest("Department name is required.");
            }

            try
            {
                var doctors = _doctorServicee.GetDoctorsByDepartmentName(departmentName);
                return Ok(doctors);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }


        }
        //[Authorize(Roles = "admin,doctor")]
        [HttpPut("UpdateDoctorDetails")]
        public IActionResult UpdateDoctorDetails([FromBody] DoctorUpdateDTO input)
        {
            try
            {
                // Extract the token from the request
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId = int.Parse(JwtHelper.GetClaimValue(token, "sub")); // Logged-in user's ID

                // Check if the user's role allows them to perform this action
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin" && userRole != "superAdmin" && userRole != "doctor"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }

                // If the user is a doctor, ensure they can only update their own details
                if (userRole == "doctor")
                {
                    input.DID= userId; // Ensure the doctor can only update their own details
                }

                // Validate the input
                if (input == null)
                {
                    return BadRequest(new { message = "Updated doctor details are required." });
                }

                if (input.DID <= 0)
                {
                    return BadRequest(new { message = "Invalid Doctor ID." });
                }

                // Call the service layer to update doctor details
                _doctorServicee.UpdateDoctorDetails(input.DID, input);

                return Ok(new { message = "Doctor details updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}
