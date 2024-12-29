using HospitalSystemTeamTask.DTO_s;
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

        [HttpGet("{branchName}")]
        public ActionResult<BranchDTO> GetBranchDetails(string branchName)
        {
            try
            {
                var branchDetails = _branchService.GetBranchDetailsByBranchName(branchName);
                return Ok(branchDetails);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle the case where the branch is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving the branch details.", error = ex.Message });
            }
        }

        [HttpPut("{branchName}")]
        public IActionResult UpdateBranch(string branchName, [FromBody] BranchDTO updatedBranchDto)
        {
            try
            {
                _branchService.UpdateBranch(branchName, updatedBranchDto);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the branch.", error = ex.Message });
            }
        }

    }
}
