using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

namespace HospitalSystemTeamTask.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public void AddBranch(BranchDTO branchDto)
        {
            var branch = new Branch
            {
                BranchName = branchDto.BranchName,
                Location = branchDto.Location,
                IsActive = true // Default new branches to active
            };
            _branchRepository.AddBranch(branch);
        }
    }
}
