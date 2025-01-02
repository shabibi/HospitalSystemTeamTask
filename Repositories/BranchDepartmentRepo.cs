using HospitalSystemTeamTask.Models;
using System.Linq;

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

        public IEnumerable<Department> GetDepartmentsByBranch(int BranchID)
        {
            try
            {
                // Query the departments associated with the branch
                return _context.branchDepartments
                               .Where(b => b.BID == BranchID)
                               .Select(b => b.Department) // Assuming a navigation property exists
                               .ToList(); // Execute the query
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }

        }

        public IEnumerable<Branch> GetBranchByDepartments(int departmentId)
        {
            try
            {
                // Query the branches associated with the department
                return _context.branchDepartments
                               .Where(b => b.DepID == departmentId)
                               .Select(b => b.Branch) // Assuming navigation property
                               .ToList(); // Materialize the results
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }

        }
        public BranchDepartment GetBranchDep (int departmentId, int branchId)
        {
            try
            {
                return _context.branchDepartments
                   .FirstOrDefault(bd => bd.BID == branchId && bd.DepID == departmentId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }

        }
        

        public void UpdateBranchDepartment(BranchDepartment branchDepartment)
        {
            try
            {
                // Validate that the branch-department combination exists
                var existingBranchDepartment = GetBranchDep(branchDepartment.BID, branchDepartment.DepID);

                if (existingBranchDepartment != null) 
                  _context.Update(existingBranchDepartment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
    }
}
