using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystemTeamTask.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new branch
        public void AddBranch(Branch branch)
        {
            try
            {
                _context.Branches.Add(branch);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public IEnumerable<Branch> GetAllBranches()
        {
            return _context.Branches.ToList();
        }

        public Branch GetBranchByBranchName(string branchName)
        {

            return _context.Branches
        .FirstOrDefault(b => b.BranchName.ToLower() == branchName.ToLower());
        }

        public void UpdateBranch(Branch branch)
        {
            _context.Branches.Update(branch);
            _context.SaveChanges();
        }
        public Branch GetBranchById(int id)  
        {
            return _context.Branches
                .FirstOrDefault(b => b.BID == id);
        }

    }
}
