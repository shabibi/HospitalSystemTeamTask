
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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _PatientService;
      
        private readonly IConfiguration _configuration;

        public PatientController(IPatientService patientService, IConfiguration configuration)
        {
            _PatientService = patientService;
            _configuration = configuration;
        }

        [HttpPost("AddPatient")]
        public IActionResult AddPatient(PatientInputDTO input)
        {
            try
            {

                // Add the patient
                _PatientService.AddPatient(input);


                return Ok("Patient added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetPatientData")]
        public IActionResult GetPatientData(string? userName, int? uid)
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                if (string.IsNullOrEmpty(userRole))
                {
                    return Unauthorized(new { message = "You are not authorized to perform this action." });
                }

                var patientData = _PatientService.GetPatientData(userName, uid);
                // Check if the user's role allows them to perform this action
                if (userRole == "patient" && userId != patientData.PID)
                {
                    return Unauthorized(new { message = "You are not authorized to view another patient's data." });
                }


                return Ok(patientData);
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("UpdatePatientphoneNumber")]
        public IActionResult UpdatePatientDetails( int UID, string phoneNumber)
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                // Check if the user's role allows them to perform this action
                if (string.IsNullOrEmpty(userRole) ||
            !(userRole == "admin" || userRole == "superAdmin" || (userRole == "patient" && userId == UID)))
                {
                    return BadRequest("You are not authorized to perform this action.");
                }

                if ( phoneNumber == null)
                {
                    return BadRequest("Patient phone Number are required.");
                }
                if (UID <= 0)
                {
                    return BadRequest("Valid Patient ID (PID) is required.");
                }


                _PatientService.UpdatePatientDetails(UID,phoneNumber);

                return Ok("Patient phone Number updated successfully.");

            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }
        
        //addmin
 
    
    }
}
