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
            {
                try
                {
                    // Get all branches from the repository
                    var branches = _branchRepository.GetAllBranches();
                    List<BranchDTO> branchDTOs = new List<BranchDTO>();

                    foreach (var branch in branches)
                    {
                        // Map the branch to BranchDTO and add it to the list
                        branchDTOs.Add(new BranchDTO
                        {
                            BranchName = branch.BranchName,
                            Location = branch.Location,
                            Status = branch.IsActive ? "Active" : "Inactive" 
                        });
                    }

                   
                    return branchDTOs;
                }
                catch (Exception ex)
                {
               
                    Console.WriteLine($"An error occurred while fetching branches: {ex.Message}");

                    throw new ApplicationException("An error occurred while fetching branches.", ex);
                }
            }
        }

        public BranchDTO GetBranchDetailsByBranchName(string branchName)
        {
            
            var branch = _branchRepository.GetBranchByBranchName(branchName);

            if (branch == null)
            {
               
                throw new KeyNotFoundException($"Branch with name '{branchName}' not found.");
            }

          
            return new BranchDTO
            {
                BranchName = branch.BranchName,
                Location = branch.Location,
                Status = branch.IsActive ? "Active" : "Inactive"
            };
        }

        public void UpdateBranch(string branchName, BranchDTO updatedBranchDto)
        {
     
            var branch = _branchRepository.GetBranchByBranchName(branchName);

            if (branch == null)
            {
                throw new KeyNotFoundException($"Branch with name '{branchName}' not found.");
            }

            // Update the branch's details
            branch.BranchName = updatedBranchDto.BranchName ?? branch.BranchName;
            branch.Location = updatedBranchDto.Location ?? branch.Location;

            
            if (!string.IsNullOrEmpty(updatedBranchDto.Status))
            {
                branch.IsActive = updatedBranchDto.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
            }

            // Save the updated branch
            _branchRepository.UpdateBranch(branch);
        }

    }
}
