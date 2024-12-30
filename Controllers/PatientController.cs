
using HospitalSystemTeamTask.DTO_s;
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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _PatientService;
      
        private readonly IConfiguration _configuration;

        public PatientController(IPatientService patientService, IConfiguration configuration)
        {
            _PatientService = patientService;
            _configuration = configuration;
        }











        [AllowAnonymous]
        [HttpGet("GetPatientById/{PID}")]
        public IActionResult GetPatientById(int PID)
        {
            try
            {
                var patient = _PatientService.GetPatientById(PID);
                return Ok(patient);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving patient. {(ex.Message)}");
            }
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


        [AllowAnonymous]
        [HttpPut("UpdatePatientDetails")]
        public IActionResult UpdatePatientDetails( int UID, PatientUpdate input)
        {
            try
            {
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
        // [Authorize(Roles = "Patient")]
        [AllowAnonymous]
        [HttpPost("AddPatient")]
        public IActionResult AddPatient( PatientInputDTO input)
        {
            try
            {
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

    }
}
