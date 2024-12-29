using HospitalSystemTeamTask.DTO_s;

namespace HospitalSystemTeamTask.Services
{
    public interface IBranchService
    {
        void AddBranch(BranchDTO branchDto);
        IEnumerable<BranchDTO> GetAllBranches();
        BranchDTO GetBranchDetailsByBranchName(string branchName);
        void UpdateBranch(string branchName, BranchDTO updatedBranchDto);
    }
}