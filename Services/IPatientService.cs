using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int Pid);
        void UpdatePatientDetails(Patient updatedPatient);
    }
}
