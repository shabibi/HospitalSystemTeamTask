using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using HospitalSystemTeamTask.Repositories;
namespace HospitalSystemTeamTask.Repositories
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            try
            {
                return _context.Doctors.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public Doctor GetDoctorById(int Did)
        {
            try
            {
                return _context.Doctors.FirstOrDefault(u => u.DID == Did);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

    }
}

