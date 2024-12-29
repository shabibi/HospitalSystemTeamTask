using HospitalSystemTeamTask.Models;

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


    }
}
