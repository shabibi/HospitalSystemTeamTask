using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpPost]
        public IActionResult AddBranch([FromBody] BranchDTO branchDto)
        {
            try
            {
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


        [HttpPatch("{branchName}")]
        public IActionResult UpdateBranch(string branchName, [FromBody] UpdateBranchDTO updatedBranchDto)
        {
            try
            {
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



        [HttpPatch("{branchName}/status")]
        public IActionResult SetBranchStatus(string branchName, [FromQuery] bool isActive)
        {
            try
            {
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

    }
}
