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
            _branchService.AddBranch(branchDto);
            return Ok("Branch added successfully");
        }
    }
}
