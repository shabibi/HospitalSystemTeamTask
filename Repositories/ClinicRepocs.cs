using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using System.Security.Cryptography;

namespace HospitalSystemTeamTask.Repositories
{
    public class ClinicRepo : IClinicRepocs
    {
        public ApplicationDbContext _context;
        public ClinicRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Clinic> GetAllClinic()
        {
            try
            {
                return _context.Clinics.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void AddClinic(Clinic clinic)
        {
            try
            {
                _context.Clinics.Add(clinic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
    }
}