using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();

    }
}
