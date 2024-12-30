using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using HospitalSystemTeamTask.Repositories;

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
}
