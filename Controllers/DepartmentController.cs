﻿using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Repositories;
using Microsoft.AspNetCore.Authorization;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IBranchDepartmentService _branchDepartmentService;

        public DepartmentController(IDepartmentService departmentService, IBranchDepartmentService branchDepartmentService)
        {
            _departmentService = departmentService;
            _branchDepartmentService = branchDepartmentService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateDepartment([FromBody] DepartmentDTO departmentDto)
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

                _departmentService.CreateDepartment(departmentDto);
                return Ok(new { message = "Department created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the department.", error = ex.Message });
            }
        }


        [HttpGet("GetAllDepartments")]
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

        [HttpGet("GetBranchesByDepartment")]
        public IActionResult GetBranchesByDepartment(string departmentName)
        {
            try
            {
                if (string.IsNullOrEmpty(departmentName))
                    return NotFound("Branch name required");
                var branch = _branchDepartmentService.GetBranchsByDepartment(departmentName);
                return Ok(branch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = $"No branches found for the department '{departmentName}'.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the branch details.", error = ex.Message });
            }
        }
        [Authorize]
        [HttpPatch("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] DepDTO departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Extract the token and validate the user's role
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }

                // Update the department
                _departmentService.UpdateDepartment(departmentDto);

                return Ok(new { message = "Department updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department.", error = ex.Message });
            }
        }


        [Authorize]
        [HttpPatch("{id}/set-status")]
        public IActionResult SetDepartmentStatus(int id, [FromQuery] bool isActive)
        {
            try
            {
                // Extract token and user role
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check user role
                if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }

                // Call service to set department status
                _departmentService.SetDepartmentActiveStatus(id, isActive);
                var statusMessage = isActive ? "activated" : "deactivated";
                return Ok(new { message = $"Department {statusMessage} successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department status.", error = ex.Message });
            }
        }



    }
}
