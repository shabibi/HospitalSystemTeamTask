using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("branch")]
        public IActionResult GetBranchDetails([FromQuery] string? branchName, [FromQuery] int? branchId)
        {
            // Validate input: only one parameter should be provided
            if (!string.IsNullOrEmpty(branchName) && branchId.HasValue)
            {
                return BadRequest(new { message = "Only one parameter (branchName or branchId) can be provided at a time." });
            }

            try
            {
                // Call the service to get branch details
                var branch = _branchService.GetBranchDetails(branchName, branchId);

                // Check if the branch is found
                if (branch == null)
                {
                    return NotFound(new { message = "Branch not found." });
                }

                return Ok(branch);
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if no branch is found
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Return 400 for invalid input
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 for any unexpected errors
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
        [HttpPatch("UpdateDepartment/{branchId}")]
        public IActionResult UpdateBranch(int branchId, [FromBody] UpdateBranchDTO updatedBranchDto)
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
                _branchService.UpdateBranch(branchId, updatedBranchDto);
                return Ok(new { message = $"Branch with ID ' {branchId}' updated successfully." });
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
        [HttpPatch("{branchId}/status")]
        public IActionResult SetBranchStatus(int branchId, [FromQuery] bool isActive)
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
                _branchService.SetBranchStatus(branchId, isActive);
                return Ok(new { message = $"Branch with ID '{branchId}' status updated to {(isActive ? "Active" : "Inactive")}." });
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

        //[Authorize]
        //[HttpPost("AddDepartmentToBranch")]
        //public IActionResult AddDepartmentToBranch (BranchDepDTO branchDepartment)
        //{
        //    try
        //    {
        //        if (branchDepartment == null)
        //            return BadRequest("data is required.");

        //        string token = JwtHelper.ExtractToken(Request);
        //        var userRole = JwtHelper.GetClaimValue(token, "unique_name");

        //        // Check if the user's role allows them to perform this action
        //        if (userRole == null || (userRole != "admin" && userRole != "superAdmin"))
        //        {
        //            return BadRequest(new { message = "You are not authorized to perform this action." });
        //        }
        //        _branchDepartmentService.AddDepartmentToBranch(branchDepartment);

        //        return Ok("Department added to Branch successfully");

        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        // Handle specific exceptions, e.g., null input
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        // Handle validation-related exceptions
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a generic error response
        //        return StatusCode(500, $"An error occurred while adding the Department to Branch: {ex.Message}");
        //    }
        //}

    }
}
