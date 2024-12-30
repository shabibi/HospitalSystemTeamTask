using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{

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
        public IActionResult AddClinic(ClinicInput input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("clinic details are required.");
                }



                _clinicService.AddClinic(input);


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
        //[Authorize(Roles = "Admin,Doctor")]
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

        [HttpGet("GetClinicsByBranchName/{branchName}")]
        public IActionResult GetClinicsByBranchName(string branchName)
        {
            try
            {
                var clinics = _clinicService.GetClinicsByBranchName(branchName);
                return Ok(clinics);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
        [HttpGet("GetClinicsByDepartmentID/{departmentId}")]
        public IActionResult GetClinicsByDepartmentID(int departmentId)
        {
            try
            {
                var clinics = _clinicService.GetClinicsByDepartmentId(departmentId);
                return Ok(clinics);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPut("UpdateClinicDetails/{CID}")]
        public IActionResult UpdateClinicDetails(int CID,  ClinicInput input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("Updated clinic details are required.");
                }

                if (CID <= 0)
                {
                    return BadRequest("Invalid Clinic ID.");
                }

                _clinicService.UpdateClinicDetails(CID, input);
                return Ok("Clinic details updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

    }
}
