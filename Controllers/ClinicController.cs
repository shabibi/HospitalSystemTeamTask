using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllClinics")]
        public IActionResult GetAllClinics()
        {
            try
            {
                var clinics = _clinicService.GetAllClinic();
                return Ok(clinics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddClinic")]
        public IActionResult AddClinic([FromBody] Clinic clinic)
        {
            try
            {
                if (clinic == null)
                {
                    return BadRequest("Clinic details are required.");
                }

                // Call service to add the clinic
                _clinicService.AddClinic(clinic);

                return Ok("Clinic added successfully.");
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
