using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IPatientRepo
    {
       
        IEnumerable<Patient> GetAllPatients();
       

    }
}
