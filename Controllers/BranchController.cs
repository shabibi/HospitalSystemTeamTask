using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IBranchDepartmentService _branchDepartmentService;
        public BranchController(IBranchService branchService, IBranchDepartmentService branchDepartmentService)
        {
            _branchService = branchService;
            _branchDepartmentService = branchDepartmentService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddBranch([FromBody] BranchDTO branchDto)
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
                _branchService.AddBranch(branchDto);
                return Ok("Branch added successfully");
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the new Branch: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public ActionResult<BranchDTO> GetBranchById(int id)
        {
            var branchDto = _branchService.GetBranchById(id);
            if (branchDto == null)
            {
                return NotFound();
            }
            return Ok(branchDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<BranchDTO>> GetAllBranches()
        {
            try
            {
                var branches = _branchService.GetAllBranches();
                return Ok(branches); // Return the list of branches as a successful response
            }
            catch (ApplicationException ex)
            {
                // Handle known exceptions
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("details/{branchName}")]
        public IActionResult GetBranchDetails(string branchName)
        {
            try
            {
                var branch = _branchService.GetBranchDetailsByBranchName(branchName);
                return Ok(branch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the branch details.", error = ex.Message });
            }
        }


        [HttpGet("GetDepartmentsByBranch")]
        public IActionResult DepartmentsByBranch(string branchName)
        {
            try
            {
                if(string.IsNullOrEmpty(branchName))
                    return NotFound("Branch name required");
                var branch = _branchDepartmentService.GetDepartmentsByBranch(branchName);
                return Ok(branch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the branch details.", error = ex.Message });
            }
        }
        [Authorize]
        [HttpPatch("{branchName}")]
        public IActionResult UpdateBranch(string branchName, [FromBody] UpdateBranchDTO updatedBranchDto)
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

                // Call the service to update the branch
                _branchService.UpdateBranch(branchName, updatedBranchDto);
                return Ok(new { message = $"Branch '{branchName}' updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if branch is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 for any other errors
                return StatusCode(500, new { message = "An error occurred while updating the branch.", error = ex.Message });
            }
        }


        [Authorize]
        [HttpPatch("{branchName}/status")]
        public IActionResult SetBranchStatus(string branchName, [FromQuery] bool isActive)
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

                // Call the service to set the status
                _branchService.SetBranchStatus(branchName, isActive);
                return Ok(new { message = $"Branch '{branchName}' status updated to {(isActive ? "Active" : "Inactive")}." });
            }
            catch (KeyNotFoundException ex)
            {
                // If the branch is not found, return a 404
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // If any other error occurs, return a 500
                return StatusCode(500, new { message = "An error occurred while updating the branch status.", error = ex.Message });
            }


        }

        [Authorize]
        [HttpPost("AddDepartmentToBranch")]
        public IActionResult AddDepartmentToBranch (BranchDepDTO branchDepartment)
        {
            try
            {
                if (branchDepartment == null)
                    return BadRequest("data is required.");

                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }
                _branchDepartmentService.AddDepartmentToBranch(branchDepartment);

                return Ok("Department added to Branch successfully");

            }
            catch (ArgumentNullException ex)
            {
                // Handle specific exceptions, e.g., null input
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                // Handle validation-related exceptions
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the Department to Branch: {ex.Message}");
            }
        }

    }
}
