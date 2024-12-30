using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IDoctorRepo
    {
        IEnumerable<Doctor> GetAllDoctors();
    }
}
