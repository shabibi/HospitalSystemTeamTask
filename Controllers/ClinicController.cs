using HospitalSystemTeamTask.DTO_s;
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

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("AddClinic")]
        public IActionResult AddClinic(ClinicInput clinicDto)
        {
            try
            {
                if (clinicDto == null)
                {
                    return BadRequest("Clinic details are required.");
                }

                // Map DTO to Clinic entity
                var clinic = new Clinic
                {
                    //DepID = clinicDto.DepID,
                    //AssignDoctor = clinicDto.AssignDoctor,
                    BID = clinicDto.BID,
                    ClincName = clinicDto.ClincName,
                    Capacity = clinicDto.Capacity,
                    StartTime = clinicDto.StartTime,
                    EndTime = clinicDto.EndTime,
                    SlotDuration = clinicDto.SlotDuration,
                    Cost = clinicDto.Cost,
                    IsActive = clinicDto.IsActive
                };

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
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetClinicById/{CID}")]
        public IActionResult GetClinicById(int CID)
        {
            try
            {
                var clinic = _clinicService.GetClinicById(CID);
                return Ok(clinic);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving patient. {(ex.Message)}");
            }
        }

        [HttpGet("GetClinicByName/{ClincName}")]
        public IActionResult GetClinicByName(string ClincName)
        {
            try
            {
                var clinic = _clinicService.GetClinicByName(ClincName);
                return Ok(clinic);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving patient. {(ex.Message)}");
            }
        }
    }
}
