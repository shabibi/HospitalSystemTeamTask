using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{


    public interface IBranchService
    {
        void AddBranch(BranchDTO branchDto);
        IEnumerable<Branch> GetAllBranches();

        Branch GetBranchDetailsByBranchName(string branchName);
        void UpdateBranch(int branchId, UpdateBranchDTO updatedBranchDto);
        void SetBranchStatus(int branchId, bool isActive);
        BranchDTO GetBranchById(int id);
        string GetBranchName(int branchId);
        BranchDTO GetBranchDetails(string? branchName, int? branchId);
    }

    
}