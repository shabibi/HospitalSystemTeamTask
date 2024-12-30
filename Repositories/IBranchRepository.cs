using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IBranchRepository
    {
        void AddBranch(Branch branch);
        IEnumerable<Branch> GetAllBranches();
        Branch GetBranchByBranchName(string branchName);
        void UpdateBranch(Branch branch);
        Branch GetBranchById(int id);
    }
}