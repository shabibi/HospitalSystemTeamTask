using HospitalSystemTeamTask.Models;

using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepocs _clinicRepo;

        public ClinicService(IClinicRepocs clinicRepo)
        {
            _clinicRepo = clinicRepo;
        }
        public IEnumerable<Clinic> GetAllClinic()
        {
            return _clinicRepo.GetAllClinic();
        }
   
    }
}
