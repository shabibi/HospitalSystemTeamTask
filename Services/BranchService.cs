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

        public IEnumerable<BranchDTO> GetAllBranches()
        {
            try
            {
                // Get all branches from the repository
                var branches = _branchRepository.GetAllBranches();

                // Map the branches to BranchDTOs
                return branches.Select(branch => new BranchDTO
                {
                    BranchName = branch.BranchName,
                    Location = branch.Location
                }).ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred while fetching branches: {ex.Message}");


                throw new ApplicationException("An error occurred while fetching branches.", ex);
            }
        }

    }
}
