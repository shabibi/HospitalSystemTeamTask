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


        public void AddClinic(Clinic clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentException("Clinic details are required.");
            }

            if (string.IsNullOrEmpty(clinic.ClincName))
            {
                throw new ArgumentException("Clinic name is required.");
            }

            if (clinic.Capacity <= 0)
            {
                throw new ArgumentException("Clinic capacity must be greater than 0.");
            }

            if (clinic.StartTime >= clinic.EndTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            // Call repository to add the clinic
            _clinicRepo.AddClinic(clinic);
        }
        public Clinic GetClinicById(int Cid)
        {
            var clinic = _clinicRepo.GetClinicById(Cid);
            if (clinic == null)
                throw new KeyNotFoundException($"clinic with ID {Cid} not found.");
            return clinic;
        }

        public Clinic GetClinicByName(string clinicName)
        {
            var clinic = _clinicRepo.GetClinicByName(clinicName);
            if (clinic == null)
                throw new KeyNotFoundException($"clinic with name {clinicName} not found.");
            return clinic;
        }

        public IEnumerable<Clinic> GetClinicsByBranchName(string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
            {
                throw new ArgumentException("Branch name is required.");
            }

            var clinics = _clinicRepo.GetClinicsByBranchName(branchName);
            if (!clinics.Any())
            {
                throw new KeyNotFoundException($"No clinics found for branch: {branchName}");
            }

            return clinics;
        }

    }
}
