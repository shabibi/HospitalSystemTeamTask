
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalSystemTeamTask.Controllers
{
    [Authorize]
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


        [Authorize(Roles = "Patient")]
        [HttpPut("UpdatePatientDetails")]
        public IActionResult UpdatePatientDetails(Patient updatedPatient)
        {
            try
            {
                if (updatedPatient == null)
                {
                    return BadRequest("Patient details are required.");
                }

                // Extract user ID from the token to ensure the patient is updating their own details
                var tokenUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (updatedPatient.PID != tokenUserId)
                {
                    return Unauthorized("You can only update your own details.");
                }

                // Update patient details
                _PatientService.UpdatePatientDetails(updatedPatient);

                return Ok("Patient details updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [Authorize(Roles = "Patient")]
        
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
                var patient = new Patient
                {
                    User = new User
                    {
                        UserName = input.UserName,
                        Email = input.Email,
                        Password = input.Password,
                        Role = "Patient",
                        IsActive = true
                    },
                    Age = input.Age,
                    Gender = input.Gender
                };

                // Add the patient
                _PatientService.AddPatient(patient);

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
