using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public class BranchDepartmentRepo : IBranchDepartmentRepo
    {
        private readonly ApplicationDbContext _context;
        public BranchDepartmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddDepartmentToBranch(BranchDepartment branchDepartment)
        {
            try
            {
                _context.branchDepartments.Add(branchDepartment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
    }
}
