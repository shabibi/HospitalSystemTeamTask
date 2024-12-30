using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepocs _clinicRepo;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;
        public ClinicService(IClinicRepocs clinicRepo, IBranchService branchService, IDepartmentService departmentService)
        {
            _clinicRepo = clinicRepo;
            _branchService = branchService;
           _departmentService = departmentService;
        }
        public IEnumerable<Clinic> GetAllClinic()
        {
            return _clinicRepo.GetAllClinic();
        }


        public void AddClinic(ClinicInput input)
        {
            if (input == null)
            {
                throw new ArgumentException("Clinic details are required.");
            }

            TimeSpan totalDuration = input.EndTime - input.StartTime;
            if (input.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than 0.");
            }

            int slotTime = (int)(totalDuration.TotalMinutes / input.Capacity);

            var clinic = new Clinic
            {
                ClincName = input.ClincName,
                Capacity = input.Capacity,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                SlotDuration = slotTime,
                Cost = input.Cost,
                IsActive = input.IsActive,
                DepID = input.DepID,
                BID = input.BID,
                AssignDoctor = input.AssignDoctor
            };
            _clinicRepo.AddClinic(clinic);
        }

        public Clinic GetClinicById(int clinicId)
        {
            // Validate input
            if (clinicId <= 0)
            {
                throw new ArgumentException("Invalid clinic ID. Clinic ID must be greater than 0.");
            }

            // Retrieve clinic by ID
            var clinic = _clinicRepo.GetClinicById(clinicId);

            // Handle clinic not found
            if (clinic == null)
            {
                throw new KeyNotFoundException($"Clinic with ID {clinicId} not found.");
            }

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

        public IEnumerable<Clinic> GetClinicsByDepartmentId(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0.");
            }

            var clinics = _clinicRepo.GetClinicsByDepartmentID(departmentId);
            if (!clinics.Any())
            {
                throw new KeyNotFoundException($"No clinics found for Department ID: {departmentId}");
            }

            return clinics;
        }

    }
}

