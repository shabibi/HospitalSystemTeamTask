using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
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


        [HttpGet("GetDoctorById/{DoctorID}")]
        public IActionResult GetDoctorById(int DoctorID)
        {
            try
            {
                
               
                if (DoctorID <= 0)
                {
                    return BadRequest("Valid Doctor ID (DID) is required.");
                }
                var doctor = _doctorServicee.GetDoctorById(DoctorID);
                return Ok(doctor);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"This person not a doctor!!. {(ex.Message)}");
            }
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
        [HttpPut("UpdateDoctorDetails/{UID}/{DID}")]
        public IActionResult UpdateDoctorDetails(int UID, int DID, DoctorUpdateDTO input)
        {
            try
            {
                // Extract token and claims
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var loggedInUserId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                // Validate the user's role
                if (string.IsNullOrEmpty(userRole) ||
                    (userRole != "admin" && userRole != "superAdmin" && userRole != "doctor"))
                {
                    return BadRequest("You are not authorized to perform this action.");
                }

                // Check if the user is a doctor and not authorized to update another doctor's details
                if (userRole == "doctor" && loggedInUserId != UID)
                {
                    return BadRequest("You are not authorized to update details of another doctor.");
                }

                // Validate the input
                if (UID <= 0 || DID <= 0)
                {
                    return BadRequest("Invalid UID or DID.");
                }

                if (input == null)
                {
                    return BadRequest("Updated doctor details are required.");
                }

                // Perform the update
                _doctorServicee.UpdateDoctorDetails(UID, DID, input);

                return Ok("Doctor details updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if the doctor is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while updating doctor details. {ex.Message}");
            }
        }


    }
}
