
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepo _PatientRepo;

        public PatientService(IPatientRepo PatientRepo)
        {
            _PatientRepo = PatientRepo;
        }



        public IEnumerable<Patient> GetAllPatients()
        {
            return _PatientRepo.GetAllPatients();
        }
        

    }

}

