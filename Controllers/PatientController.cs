
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

        [NonAction]
        public string GenerateJwtToken(string PId, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, PId),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


       
        [HttpPut("UpdatePatientDetails")]
        public IActionResult UpdatePatientDetails( int UID, PatientUpdate input)
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

                if (input == null)
                {
                    return BadRequest("Patient details are required.");
                }
                if (UID <= 0)
                {
                    return BadRequest("Valid Patient ID (PID) is required.");
                }


                _PatientService.UpdatePatientDetails(UID, input);

                return Ok("Patient details updated successfully.");

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
        [HttpPost("AddPatient")]
        public IActionResult AddPatient( PatientInputDTO input)
        {
            try
            {
                // Extract the token from the request and retrieve the user's role
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }

                if (input == null)
                {
                    return BadRequest("Patient details are required.");
                }

                // Map DTO to Patient entity
              

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
        public IActionResult GetPatientData( string? userName, int? uid)
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                // Check if the user's role allows them to perform this action
                if (string.IsNullOrEmpty(userRole) ||
            !(userRole == "doctor" || userRole == "admin" || userRole == "superAdmin" || (userRole == "patient" && userId == uid)))
                {
                    return BadRequest("You are not authorized to perform this action.");
                }
                var patientData = _PatientService.GetPatientData(userName, uid);
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

    }
}
