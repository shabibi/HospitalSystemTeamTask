using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
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
        public IActionResult GetUserById(int DoctorID)
        {
            try
            {
                var doctor = _doctorServicee.GetDoctorById(DoctorID);
                return Ok(doctor);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving doctor. {(ex.Message)}");
            }
        }

    }
}