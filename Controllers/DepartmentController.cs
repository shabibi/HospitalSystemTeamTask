using HospitalSystemTeamTask.DTO_s;
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
    }
}
