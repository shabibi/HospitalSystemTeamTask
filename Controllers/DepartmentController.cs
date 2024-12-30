using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Repositories;
using HospitalSystemTeamTask.Services;

using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
       

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] DepartmentDTO departmentDto)
        {
            try
            {
                _departmentService.CreateDepartment(departmentDto);
                return Ok(new { message = "Department created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the department.", error = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                var departments = _departmentService.GetAllDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving departments.", error = ex.Message });
            }
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] DepartmentDTO departmentDto)
        {
            try
            {
                _departmentService.UpdateDepartment(id, departmentDto);
                return Ok(new { message = "Department updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Department not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department.", error = ex.Message });
            }
        }

        [HttpPatch("{id}/set-status")]
        public IActionResult SetDepartmentStatus(int id, [FromQuery] bool isActive)
        {
            try
            {
                _departmentService.SetDepartmentActiveStatus(id, isActive);
                var statusMessage = isActive ? "activated" : "deactivated";
                return Ok(new { message = $"Department {statusMessage} successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Department not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department status.", error = ex.Message });
            }
        }
    }
}
