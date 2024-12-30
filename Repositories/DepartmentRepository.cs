using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            SaveChanges();
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
