using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IPatientRepo
    {
       
        IEnumerable<Patient> GetAllPatients();
        public Patient GetPatientsById(int Pid);
        void UpdatePatient(Patient patient);


    }
}
